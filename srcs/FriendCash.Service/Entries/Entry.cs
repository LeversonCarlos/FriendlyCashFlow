#region Using
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Entries
{
   [RoutePrefix("api/entries")]
   public class EntryController : Base.BaseController
   {

      #region Contants
      internal class Constants
      {
         internal const string ENTRY_NOTFOUND = "MSG_ENTRIES_ENTRY_NOTFOUND";
         internal const string PAYDATE_REQUIRED = "MSG_ENTRIES_PAYDATE_REQUIRED";
         internal const string ACCOUNT_REQUIRED = "MSG_ENTRIES_IDACCOUNT_REQUIRED";
         internal const string CATEGORY_REQUIRED = "MSG_ENTRIES_IDCATEGORY_REQUIRED";
         internal const string RECURRENCY_QUANTITY_REQUIRED = "MSG_RECURRENCIES_QUANTITY_REQUIRED";
      }
      #endregion

      #region Query

      internal IQueryable<Model.bindEntry> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Entries
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)            
            .AsQueryable();
      }

      internal IQueryable<Model.viewEntry> QueryView()
      {
         return this.QueryData()
            .Select(x => new Model.viewEntry()
            {
               idEntry = x.idEntry,
               Text = x.Text,
               TypeValue = x.TypeValue,
               idCategory = x.idCategory,
               idAccount = x.idAccount,
               idPattern = x.idPattern,
               idTransfer = x.idTransfer,
               SearchDate = x.SearchDate,
               DueDate = x.DueDate,
               PayDate = x.PayDate,
               EntryValue = x.Value,
               idRecurrency = x.idRecurrency,
               Paid = x.Paid,
               Sorting = x.Sorting,
               RowDate = x.RowDate
            })
            .AsQueryable();
      }

      #endregion


      #region GetAll
      [Authorize(Roles = "User,Viewer"), HttpGet]
      [Route("{year:int}/{month:int}")]
      [Route("{year:int}/{month:int}/{idAcount:long}")]
      public async Task<IHttpActionResult> GetAll(int year, int month, long idAcount = 0)
      {
         try
         {

            // QUERY
            var dataQuery = this.QueryView()
               .Where(x => x.SearchDate.Year == year && x.SearchDate.Month == month);
            if (idAcount > 0) { dataQuery = dataQuery.Where(x => x.idAccount == idAcount); }
            dataQuery = dataQuery.OrderBy(x => x.Sorting);

            // EXECUTE
            var oData = await Task.FromResult(dataQuery.ToList());

            // BALANCE
            await this.ReviewList(oData, year, month, idAcount);

            // RESULT
            return Ok(oData);
         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetInterval
      [Authorize(Roles = "User,Viewer"), HttpGet]
      [Route("interval/{year}/{month}/{account}/")]
      public async Task<IHttpActionResult> GetInterval(int year, short month, long account)
      {
         try
         {
            var resultData = new Model.viewFilter();

            // CULTURE INFO
            var acceptLanguage = this.Request.Headers.AcceptLanguage.Select(x => x.Value).FirstOrDefault();
            var cultureInfo = new System.Globalization.CultureInfo(acceptLanguage);

            // INTERVAL
            var currentDate = new DateTime(year, month, 1);
            resultData.CurrentMonth = new Model.viewFilterMonth(currentDate, cultureInfo);
            resultData.PreviousMonth = new Model.viewFilterMonth(currentDate.AddMonths(-1), cultureInfo);
            resultData.NextMonth = new Model.viewFilterMonth(currentDate.AddMonths(1), cultureInfo);

            // ACCOUNTS
            using (var accountController = new Accounts.AccountController())
            {
               var accountMessage = await accountController.GetRelatedInner();
               var accountResult = ((System.Web.Http.Results.OkNegotiatedContentResult<List<Base.viewRelated>>)accountMessage);
               resultData.AccountList = accountResult.Content;
               if (resultData.AccountList != null)
               {
                  resultData.AccountList.Insert(0, new Base.viewRelated()
                  {
                     ID = "0",
                     textValue = this.GetTranslation("LABEL_ENTRIES_FILTERACCOUNT_ALL")
                  });
                  resultData.CurrentAccount = resultData.AccountList
                     .Where(x => x.ID == account.ToString())
                     .FirstOrDefault();
               }
            }

            // RESULT
            return Ok(resultData);

         }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetPending
      [Authorize(Roles = "User,Viewer"), HttpGet]
      [Route("pending")]
      public async Task<IHttpActionResult> GetPending()
      {
         try
         {

            // LIMIT DATE
            var limitDate = DateTime.Now.AddDays(7);

            // QUERY
            var dataQuery = this.QueryView()
               .Where(x => x.SearchDate <= limitDate && x.Paid == false);
            dataQuery = dataQuery.OrderBy(x => x.Sorting);

            // EXECUTE
            var oData = await Task.FromResult(dataQuery.ToList());

            // BALANCE
            await this.ReviewList(oData);

            // RESULT
            return Ok(oData);
         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetSingle
      [Authorize(Roles = "User,Viewer")]
      [Route("{id:long}", Name = "GetEntryByID")]
      public async Task<IHttpActionResult> GetSingle(long id) 
      {
         try
         {
            var dataQuery = this.QueryView().Where(x => x.idEntry == id);
            var oData = await Task.FromResult(dataQuery.FirstOrDefault());
            return Ok(oData);
         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region ReviewList

      private async Task<bool> ReviewList(List<Model.viewEntry> value, int year=0, int month=0, long idAccount=0)
      {
         try
         {
            var idUser = this.GetUserID();

            // INITIAL BALANCE
            await this.ReviewList_InitialBalance(value, year, month, idAccount);

            // ACCOUNTS
            var accountList = this.DataContext.Accounts.Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser).ToList();

            // CATEGORIES
            var categoryIDs = value.Where(x => x.idCategory.HasValue).GroupBy(x => x.idCategory.Value).Select(x => x.Key).ToList();
            var categoryList = this.DataContext.Categories.Where(x => categoryIDs.Contains(x.idCategory)).ToList();

            // TRANSFERS
            var transferIDs = value.Where(x => !string.IsNullOrEmpty(x.idTransfer)).GroupBy(x => x.idTransfer).Select(x => x.Key).ToList();
            var transferList = this.DataContext.Entries.Where(x => transferIDs.Contains(x.idTransfer)).ToList();

            // RECURRENCY
            var recurrencyIDs = value.Where(x => x.idRecurrency.HasValue).GroupBy(x => x.idRecurrency.Value).Select(x => x.Key).ToList();
            var recurrencyList = this.DataContext.Recurrencies.Where(x => recurrencyIDs.Contains(x.idRecurrency)).ToList();

            // APPLY
            double balanceValue = 0;
            foreach (var entry in value)
            {

               // BALANCE
               entry.EntryValue = Math.Round(entry.EntryValue * (entry.Type == Model.enEntryType.Income ? 1 : -1), 2);
               balanceValue += entry.EntryValue;
               entry.Balance = balanceValue;

               // STATE
               if (entry.Paid) { entry.State = Model.viewEntry.enEntryState.Paid; }
               else if (entry.DueDate < DateTime.Now) { entry.State = Model.viewEntry.enEntryState.Overdue; }

               // TRANSFER
               if (!string.IsNullOrEmpty(entry.idTransfer))
               {

                  // TRANSFER INCOME
                  var transferIncome = transferList.Where(x => x.idTransfer == entry.idTransfer && x.Type == Model.enEntryType.Income).FirstOrDefault();
                  if (transferIncome != null)
                  {
                     entry.idEntryIncome = transferIncome.idEntry;
                     if (transferIncome.idAccount.HasValue && entry.idAccount > 0)
                     {
                        var idAccountModel = accountList.Where(x => x.idAccount == transferIncome.idAccount).FirstOrDefault();
                        entry.idAccountIncome = idAccountModel.idAccount;
                        entry.idAccountIncomeView = new Accounts.Model.viewAccount(idAccountModel);
                        entry.idAccountIncomeText = entry.idAccountIncomeView.Text;
                     }
                  }

                  // TRANSFER EXPENSE
                  var transferExpense = transferList.Where(x => x.idTransfer == entry.idTransfer && x.Type == Model.enEntryType.Expense).FirstOrDefault();
                  if (transferExpense != null)
                  {
                     entry.idEntryExpense = transferExpense.idEntry;
                     if (transferExpense.idAccount.HasValue && entry.idAccount > 0)
                     {
                        var idAccountModel = accountList.Where(x => x.idAccount == transferExpense.idAccount).FirstOrDefault();
                        entry.idAccountExpense = idAccountModel.idAccount;
                        entry.idAccountExpenseView = new Accounts.Model.viewAccount(idAccountModel);
                        entry.idAccountExpenseText = entry.idAccountExpenseView.Text;
                     }
                  }

                  entry.Type = Model.enEntryType.None;
               }

               // ACCOUNT
               if (entry.idAccount.HasValue && entry.idAccount > 0)
               {
                  var idAccountModel = accountList.Where(x => x.idAccount == entry.idAccount).FirstOrDefault();
                  entry.idAccountView = new Accounts.Model.viewAccount(idAccountModel);
                  entry.idAccountText = entry.idAccountView.Text;
               }

               // CATEGORY
               if (entry.idCategory.HasValue && entry.idCategory > 0)
               {
                  var idCategoryModel = categoryList.Where(x => x.idCategory == entry.idCategory).FirstOrDefault();
                  entry.idCategoryView = new Categories.Model.viewCategory(idCategoryModel);
                  entry.idCategoryText = entry.idCategoryView.HierarchyText;
               }

               // RECURRENCY
               entry.idRecurrencyView = new Entries.Model.viewRecurrency();
               if (entry.idRecurrency.HasValue && entry.idRecurrency > 0)
               {
                  var idRecurrencyModel = recurrencyList.Where(x => x.idRecurrency == entry.idRecurrency).FirstOrDefault();
                  entry.idRecurrencyView = new Entries.Model.viewRecurrency(idRecurrencyModel);
               }

            }

            // RESULT
            return true;

         }
         catch { throw; }
      }

      private async Task<bool> ReviewList_InitialBalance(List<Model.viewEntry> value, int year = 0, int month = 0, long idAcount = 0)
      {
         try
         {

            // VALIDATE
            if (year == 0 || month == 0) { return true; }

            // BALANCE
            var initialBalance = await this.getBalance(year, month, idAcount);

            // INITIAL BALANCE
            var initialDate = new DateTime(year, month, 1, 0, 0, 1);
            var initialEntry = this.ReviewList_GetEntry(initialDate, true, initialBalance.PaidValue, idAcount, "LABEL_ENTRIES_INITIAL_BALANCE");
            value.Insert(0, initialEntry);

            // OVERDUE BALANCE
            if (Math.Round(initialBalance.PaidValue, 2) != Math.Round(initialBalance.TotalValue, 2))
            {
               var overdueDate = new DateTime(year, month, 1, 0, 0, 2);
               var overdueEntry = this.ReviewList_GetEntry(overdueDate, false, Math.Round(initialBalance.TotalValue, 2) - Math.Round(initialBalance.PaidValue, 2), idAcount, "LABEL_ENTRIES_OVERDUE_BALANCE");
               value.Insert(1, overdueEntry);
            }

            // RESULT
            return true;

         }
         catch (Exception ex) { throw ex; }
      }

      private Model.viewEntry ReviewList_GetEntry(DateTime date, bool paid, double value, long account, string text)
      {

         var entry = new Model.bindEntry()
         {
            DueDate = date,
            Paid = paid,
            Text = this.GetTranslation(text),
            Value = Math.Abs(value),
            Type = (value >= 0 ? Model.enEntryType.Income : Model.enEntryType.Expense),
            RowStatus = Base.BaseModel.enRowStatus.Active
         };
         if (paid) { entry.PayDate = date; }
         if (account != 0) { entry.idAccount = account; }
         entry.RefreshSorting();

         return Model.viewEntry.Create(entry);
      }

      #endregion  


      #region Create

      [Authorize(Roles = "ActiveUser")]
      [HttpPost, Route("")]
      public async Task<IHttpActionResult> Create(Model.viewEntry value) 
      {
         try
         {

            // VALIDATE 
            if (!this.Validate(value)) { return BadRequest(ModelState); }

            // DATA
            var oData = new Model.bindEntry()
            {
               idUser = this.GetUserID(), 
               Text = value.Text,
               Type = value.Type,
               idCategory = value.idCategory,
               DueDate = value.DueDate,
               Value = Math.Abs(value.EntryValue),
               Paid = value.Paid,
               Sorting = value.Sorting,
               SearchDate = DateTime.UtcNow, 
               RowStatus = Base.BaseModel.enRowStatus.Active
            };
            if (value.Paid && value.PayDate.HasValue) { oData.PayDate = value.PayDate; }
            if (value.idAccount.HasValue) { oData.idAccount = value.idAccount; }

            // TRANSFER
            if (!string.IsNullOrEmpty(value.idTransfer))
            { oData.idTransfer = value.idTransfer; }

            // PATTERN
            var patternModel = await this.addPattern(value);
            if (patternModel != null) {
               oData.idPattern = patternModel.idPattern;
               value.idPattern = oData.idPattern;
            }

            // RECURRENCY
            var newRecurrency = false; Model.bindRecurrency recurrencyModel = null;
            if (value.idRecurrency.HasValue && value.idRecurrency.Value != 0) { oData.idRecurrency = value.idRecurrency.Value; }
            else {
               recurrencyModel = await this.addRecurrency(value);
               if (recurrencyModel != null) { oData.idRecurrency = recurrencyModel.idRecurrency; newRecurrency = true; }
            }

            // SAVE
            this.DataContext.Entries.Add(oData);
            await this.DataContext.SaveChangesAsync();

            // SORTING and BALANCE
            oData.RefreshSorting();
            await this.addBalance(oData);
            await this.DataContext.SaveChangesAsync();

            // RECURRENCY THREAD
            if (newRecurrency && recurrencyModel != null) {
               await this.generateRecurrency(recurrencyModel);
            }

            // RESULT
            var locationHeader = ""; // new Uri(Url.Link("GetEntryByID", new { id = oData.idEntry }));
            return Created(locationHeader, Model.viewEntry.Create(oData));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      #endregion

      #region Update
      [Authorize(Roles = "ActiveUser")]
      [HttpPut, Route("")]
      public async Task<IHttpActionResult> Update(Model.viewEntry value) 
      {
         try
         {

            // VALIDATE 
            if (!this.Validate(value)) { return BadRequest(ModelState); }

            // LOCATE
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idEntry == value.idEntry).FirstOrDefault());
            if (oData == null) { this.AddModelError( Constants.ENTRY_NOTFOUND); return BadRequest(ModelState); }

            // BALANCE
            await this.removeBalance(oData);

            // PATTERN
            if (oData.idPattern != value.idPattern)
            {
               await this.removePattern(oData.idPattern.Value);
               var patternModel = await this.addPattern(value);
               if (patternModel != null) {
                  oData.idPattern = patternModel.idPattern;
                  value.idPattern = oData.idPattern;
               }
            }

            // APPLY
            oData.Text = value.Text;
            oData.idCategory = value.idCategory;
            oData.DueDate = value.DueDate;
            oData.Value = Math.Abs(value.EntryValue);
            oData.Paid = value.Paid;
            if (value.Paid && value.PayDate.HasValue) { oData.PayDate = value.PayDate; } else { oData.PayDate = null; }
            if (value.idAccount.HasValue) { oData.idAccount = value.idAccount; }
            oData.RefreshSorting();

            // RECURRENCY
            await this.setRecurrency(value);

            // BALANCE
            await this.addBalance(oData);

            // SAVE
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok(Model.viewEntry.Create(oData));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Delete
      [Authorize(Roles = "ActiveUser")]
      [HttpDelete, Route("{id:long}")]
      public async Task<IHttpActionResult> Delete(long id) 
      {
         try
         {

            // LOCATE
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idEntry == id).FirstOrDefault());
            if (oData == null) { this.AddModelError(Constants.ENTRY_NOTFOUND); return BadRequest(ModelState); }

            // PATTERN
            if (oData.idPattern.HasValue)
            { await this.removePattern(oData.idPattern.Value); }

            // BALANCE
            await this.removeBalance(oData);

            // APPLY
            oData.RowStatus = Base.BaseModel.enRowStatus.Removed;
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region RemoveUserData
      [Authorize(Roles = "Admin")]
      // [HttpDelete, Route("{id:long}")]
      internal async Task<bool> RemoveUserData(string idUser)
      {
         try
         {
            var iDelete = await this.DataContext.ObjectContext.ExecuteStoreCommandAsync(string.Format("delete from v5_dataEntries where idUser='{0}'", idUser));
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion


      #region Validate
      private bool Validate(Model.viewEntry value)
      {

         // PAID DATA
         if (value.Paid)
         {
            if (value.PayDate.HasValue == false || value.PayDate.Value == DateTime.MinValue) { this.AddModelError(Constants.PAYDATE_REQUIRED, "PayDate"); }
            if (value.idAccount.HasValue == false || value.idAccount.Value == 0) { this.AddModelError(Constants.ACCOUNT_REQUIRED, "idAccount"); }
         }

         // CATEGORY
         if (value.idCategory == 0) { this.AddModelError(Constants.CATEGORY_REQUIRED, "idCategory"); }

         // RECURRENCY
         if (value.idRecurrencyView != null && value.idRecurrencyView.hasRecurrency)
         {
            if (value.idRecurrencyView.Fixed && (!value.idRecurrencyView.Quantity.HasValue || value.idRecurrencyView.Quantity.Value == 0))
            { this.AddModelError(Constants.RECURRENCY_QUANTITY_REQUIRED, "idRecurrencyView.Quantity"); }
         }

         // RESULT
         return ModelState.IsValid;
      }
      #endregion

      #region Pattern

      private async Task<Model.bindPattern> addPattern(Model.viewEntry value)
      {
         using (var patternAPI = new PatternController())
         { return await patternAPI.Add(value); }
      }

      private async Task<Model.bindPattern> removePattern(long idPattern)
      {
         using (var patternAPI = new PatternController())
         { return await patternAPI.Remove(idPattern); }
      }

      #endregion

      #region Recurrency

      private async Task<Model.bindRecurrency> addRecurrency(Model.viewEntry value)
      {
         using (var recurrencyAPI = new RecurrencyController())
         { return await recurrencyAPI.Add(value); }
      }

      private async Task<Model.bindRecurrency> setRecurrency(Model.viewEntry value)
      {
         using (var recurrencyAPI = new RecurrencyController())
         { return await recurrencyAPI.Set(value); }
      }

      private async Task generateRecurrency(Model.bindRecurrency value)
      {
         using (var recurrencyAPI = new RecurrencyController())
         {  await recurrencyAPI.Generate(value); return; }
      }

      #endregion 

      #region Balance

      private async Task<IHttpActionResult> addBalance(Model.bindEntry value)
      {
         using (var balanceAPI = new Balances.BalanceController())
         { return await balanceAPI.Add(value); }
      }

      private async Task<IHttpActionResult> removeBalance(Model.bindEntry value)
      {
         using (var balanceAPI = new Balances.BalanceController())
         { return await balanceAPI.Remove(value); }
      }

      private async Task<Balances.Model.dataBalance> getBalance(int year, int month, long idAcount)
      {
         using (var balanceAPI = new Balances.BalanceController())
         { return await balanceAPI.Get(year, month, idAcount); }
      }

      #endregion

   }

}
