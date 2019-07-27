#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Entries
{
   [RoutePrefix("api/transfers")]
   public class TransferController : Base.BaseController
   {

      #region Contants
      internal class Constants
      {
         internal const string ENTRY_NOTFOUND = "MSG_ENTRIES_ENTRY_NOTFOUND";
         internal const string PAYDATE_REQUIRED = "MSG_ENTRIES_PAYDATE_REQUIRED";
         internal const string EXPENSE_ACCOUNT_REQUIRED = "MSG_ENTRIES_EXPENSE_ACCOUNT_REQUIRED";
         internal const string INCOME_ACCOUNT_REQUIRED = "MSG_ENTRIES_INCOME_ACCOUNT_REQUIRED";
         internal const string TEXT_TRANSFER = "ENUM_CATEGORYTYPE_TRANSFER";
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

      internal IQueryable<Model.viewTransfer> QueryView() 
      {
         var oQueryData = this.QueryData().Where(x => x.idTransfer != null);
         var oQueryView =
            from e in oQueryData.Where(EXPENSE => EXPENSE.TypeValue == (short)Model.enEntryType.Expense)
            join i in oQueryData.Where(INCOME => INCOME.TypeValue == (short)Model.enEntryType.Income)
               on e.idTransfer equals i.idTransfer
            select new Model.viewTransfer()
            {
               idTransfer = e.idTransfer,
               idEntryExpense = e.idEntry,
               idEntryIncome = i.idEntry,
               idAccountExpense = e.idAccount,
               idAccountIncome = i.idAccount,
               SearchDate = e.SearchDate,
               DueDate = e.DueDate,
               PayDate = e.PayDate,
               Value = e.Value,
               Paid = e.Paid,
               Sorting = e.Sorting
            };
         return oQueryView.AsQueryable();
      }

      #endregion


      #region GetAll
      [Authorize(Roles = "User,Viewer"), HttpGet]
      [Route("{year:int}/{month:int}")]
      public async Task<IHttpActionResult> GetAll(int year, int month) 
      {
         try
         {
            var oQuery = this.QueryView()
               .Where(x => x.SearchDate.Year == year && x.SearchDate.Month == month);
            var oData = await Task.FromResult(oQuery.ToList());
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetSingle
      [Authorize(Roles = "User,Viewer")]
      [Route("{id:guid}", Name = "GetTransferByID")]
      public async Task<IHttpActionResult> GetSingle(string id)
      {
         try
         {
            var oQuery = this.QueryView().Where(x => x.idTransfer == id);
            var oData = await Task.FromResult(oQuery.FirstOrDefault());
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion


      #region Create

      [Authorize(Roles = "ActiveUser")]
      [HttpPost, Route("")]
      public async Task<IHttpActionResult> Create(Model.viewTransfer value)
      {
         try
         {

            // VALIDATE 
            if (!this.Validate(value)) { return BadRequest(ModelState); }

            // DATA
            var idTransfer = Guid.NewGuid().ToString("N");
            var oDataExpense = this.Create_Data(value, Model.enEntryType.Expense, idTransfer);
            var oDataIncome = this.Create_Data(value, Model.enEntryType.Income, idTransfer);

            // APPLY
            this.DataContext.Entries.Add(oDataExpense);
            this.DataContext.Entries.Add(oDataIncome);
            await this.DataContext.SaveChangesAsync();

            // SORTING
            oDataExpense.RefreshSorting();
            oDataIncome.RefreshSorting();
            await this.addBalance(oDataExpense);
            await this.addBalance(oDataIncome);
            await this.DataContext.SaveChangesAsync();

            // RESULT
            var locationHeader = ""; // new Uri(Url.Link("GetTransferByID", new { id = idTransfer }));
            return Created(locationHeader, Model.viewTransfer.Create(oDataExpense, oDataIncome));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      private Model.bindEntry Create_Data(Model.viewTransfer value, Model.enEntryType iType, string idTransfer)
      {
         var oData = new Model.bindEntry()
         {
            idUser = this.GetUserID(),
            Type = iType,
            idTransfer = idTransfer,
            DueDate = value.DueDate,
            Value = value.Value,
            Paid = value.Paid,
            SearchDate = DateTime.UtcNow,
            Text = value.Text,
            RowStatus = Base.BaseModel.enRowStatus.Active
         };
         if (value.PayDate.HasValue) { oData.PayDate = value.PayDate; }
         if (iType == Model.enEntryType.Expense) { oData.idAccount = value.idAccountExpense; }
         if (iType == Model.enEntryType.Income) { oData.idAccount = value.idAccountIncome; }

         return oData;
      }

      #endregion

      #region Update
      [Authorize(Roles = "ActiveUser")]
      [HttpPut, Route("")]
      public async Task<IHttpActionResult> Update(Model.viewTransfer value)
      {
         try
         {

            // VALIDATE 
            if (!this.Validate(value)) { return BadRequest(ModelState); }

            // LOCATE
            var oDataExpense = await Task.FromResult(this.QueryData().Where(x => x.idEntry == value.idEntryExpense).FirstOrDefault());
            if (oDataExpense == null) { this.AddModelError(Constants.ENTRY_NOTFOUND); return BadRequest(ModelState); }
            var oDataIncome = await Task.FromResult(this.QueryData().Where(x => x.idEntry == value.idEntryIncome).FirstOrDefault());
            if (oDataIncome == null) { this.AddModelError(Constants.ENTRY_NOTFOUND); return BadRequest(ModelState); }

            // BALANCE
            await this.removeBalance(oDataExpense);
            await this.removeBalance(oDataIncome);

            // APPLY EXPENSE
            oDataExpense.DueDate = value.DueDate;
            oDataExpense.Value = value.Value;
            oDataExpense.Paid = value.Paid;
            oDataExpense.idAccount = value.idAccountExpense;
            if (value.PayDate.HasValue) { oDataExpense.PayDate = value.PayDate; }
            oDataExpense.RefreshSorting();
            await this.addBalance(oDataExpense);

            // APPLY INCOME
            oDataIncome.DueDate = value.DueDate;
            oDataIncome.Value = value.Value;
            oDataIncome.Paid = value.Paid;
            oDataIncome.idAccount = value.idAccountIncome;
            if (value.PayDate.HasValue) { oDataIncome.PayDate = value.PayDate; }
            oDataIncome.RefreshSorting();
            await this.addBalance(oDataIncome);

            // SAVE
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok(Model.viewTransfer.Create(oDataExpense, oDataIncome));

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region Delete

      [Authorize(Roles = "ActiveUser")]
      [HttpDelete, Route("")]
      public async Task<IHttpActionResult> Delete(Model.viewTransfer value)
      { return await this.Delete(value.idTransfer); }

      [Authorize(Roles = "ActiveUser")]
      [HttpDelete, Route("{id:guid}")]
      public async Task<IHttpActionResult> Delete(string id)
      {
         try
         {

            // LOCATE
            var oData = await Task.FromResult(this.QueryData().Where(x => x.idTransfer == id).ToList());
            if (oData == null || oData.Count != 2) { this.AddModelError(Constants.ENTRY_NOTFOUND); return BadRequest(ModelState); }

            // BALANCE
            await this.removeBalance(oData[0]);
            await this.removeBalance(oData[1]);

            // APPLY 
            oData.ForEach(x => x.RowStatus = Base.BaseModel.enRowStatus.Removed);
            await this.DataContext.SaveChangesAsync();

            // RESULT
            return Ok();

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }

      #endregion


      #region Validate
      private bool Validate(Model.viewTransfer value)
      {

         // PAID DATA
         if (value.Paid)
         {
            if (value.PayDate.HasValue == false || value.PayDate.Value == DateTime.MinValue) { this.AddModelError(Constants.PAYDATE_REQUIRED, "PayDate"); }
         }

         // ACCOUNT
         if (value.idAccountExpense.HasValue == false || value.idAccountExpense.Value == 0) { this.AddModelError(Constants.EXPENSE_ACCOUNT_REQUIRED, "idAccountExpense"); }
         if (value.idAccountIncome.HasValue == false || value.idAccountIncome.Value == 0) { this.AddModelError(Constants.INCOME_ACCOUNT_REQUIRED, "idAccountIncome"); }

         // RESULT
         return ModelState.IsValid;
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

      #endregion

   }

}
