#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Imports
{

   [RoutePrefix("api/import")]
   public class ImportController : Base.BaseController
   {

      #region Contants
      internal class Constants
      {
         // internal const string WARNING_TEXT_DUPLICITY = "MSG_ACCOUNTS_TEXT_DUPLICITY";
      }
      #endregion

      #region Query

      internal IQueryable<Model.bindImport> QueryData()
      {
         var idUser = this.GetUserID();
         return this.DataContext.Imports
            .Where(x => x.RowStatusValue == (short)Base.BaseModel.enRowStatus.Active && x.idUser == idUser)
            .AsQueryable();
      }

      internal IQueryable<Model.viewImport> QueryView()
      {
         return this.QueryData()
            .OrderByDescending(x => x.RowDate)
            .Select(x => new Model.viewImport()
            {
               idImport = x.idImport,
               TotalAccounts = x.TotalAccounts,
               ImportedAccounts = x.ImportedAccounts,
               TotalCategories = x.TotalCategories,
               ImportedCategories = x.ImportedCategories,
               TotalEntries = x.TotalEntries,
               ImportedEntries = x.ImportedEntries,
               State = (Model.enImportState) x.StateValue,
               RowDate = x.RowDate
            })
            .AsQueryable();
      }

      #endregion


      #region GetAll
      [Authorize(Roles = "User,Viewer")]
      [Route("")]
      public async Task<IHttpActionResult> GetAll()
      {
         try
         {
            var oQuery = this.QueryView();
            var oData = await Task.FromResult(oQuery.OrderByDescending(x => x.RowDate).ToList());
            return Ok(oData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

      #region GetSingle
      [Authorize(Roles = "User,Viewer")]
      [Route("{id:long}", Name = "GetImportByID")]
      public async Task<IHttpActionResult> GetSingle(long id)
      {
         try
         {
            var oQuery = this.QueryView().Where(x => x.idImport == id);
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
      public async Task<IHttpActionResult> Create()
      {
         Model.bindImport oData = null;
         try
         {

            // INITIALIZE
            oData = new Model.bindImport() { idUser = this.GetUserID(), Message = "" };
            this.DataContext.Imports.Add(oData);
            await this.DataContext.SaveChangesAsync();

            // FILE PATH
            oData.Message += "FilePath: ";
            var filePath = await this.Create_GetFilePath();
            oData.Message += filePath + Environment.NewLine;

            // UPLOAD FILE
            oData.Message += "UploadFile: ";
            if (!await this.Create_SaveFile(filePath)) { return BadRequest("Cant upload file"); }
            oData.Message += "OK" + Environment.NewLine;

            // ASYNC IMPORT
            oData.RowStatus = Base.BaseModel.enRowStatus.Active;
            Task.Run(() => this.Import(oData, filePath));
            System.Threading.Thread.Sleep(500);

            // RESULT
            var locationHeader = ""; // new Uri(Url.Link("GetImportByID", new { id = oData.idImport}));
            return Created(locationHeader, new Model.viewImport(oData));

         }
         catch (Exception ex) {
            oData.Message += "Exception [" + ex.ToString() + "]";
            await this.DataContext.SaveChangesAsync();
            return this.GetErrorResult(ex);
         }
      }

      private async Task<string> Create_GetFilePath()
      {
         try
         {

            // PATH
            var uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");
            if (!System.IO.Directory.Exists(uploadPath))
            { System.IO.Directory.CreateDirectory(uploadPath); }

            // FILE
            var fileName = Guid.NewGuid().ToString() + ".import";

            // RESULT
            return await Task.FromResult(string.Format("{0}\\{1}", uploadPath, fileName));

         }
         catch (Exception ex) { throw ex; }
      }

      private async Task<bool> Create_SaveFile(string filePath)
      {
         try
         {

            // WRITE FILE
            var byteArray = await Request.Content.ReadAsByteArrayAsync();
            System.IO.File.WriteAllBytes(filePath, byteArray);

            // RESULT
            return System.IO.File.Exists(filePath);

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

      #region Import

      private async Task<bool> Import(Model.bindImport importModel, string filePath)
      {
         try
         {

            // DATA
            var dataList = this.Import_GetData(importModel, filePath);
            System.IO.File.Delete(filePath);
            if (dataList.Count == 0) {
               importModel.State = Model.enImportState.Canceled ;
               await this.DataContext.SaveChangesAsync();
               return false;
            }

            // LISTS
            var accounts = new List<string>();
            accounts.AddRange(dataList.Where(x => !string.IsNullOrEmpty(x.Account)).GroupBy(x => x.Account).Select(x => x.Key).ToList());
            accounts.AddRange(dataList.Where(x => !string.IsNullOrEmpty(x.AccountFrom)).GroupBy(x => x.AccountFrom).Select(x => x.Key).ToList());
            accounts.AddRange(dataList.Where(x => !string.IsNullOrEmpty(x.AccountTo)).GroupBy(x => x.AccountTo).Select(x => x.Key).ToList());
            accounts = accounts.GroupBy(x => x).Select(x => x.Key).ToList();
            var incomeCategories = dataList.Where(x => !string.IsNullOrEmpty(x.Category) && x.Type == Data.enDataType.Income).GroupBy(x => x.Category).Select(x => x.Key).ToList();
            var expenseCategories = dataList.Where(x => !string.IsNullOrEmpty(x.Category) && x.Type == Data.enDataType.Expense).GroupBy(x => x.Category).Select(x => x.Key).ToList();

            // APPLY TOTALS
            importModel.TotalEntries = dataList.Count;
            importModel.TotalAccounts = accounts.Count;
            importModel.TotalCategories = (incomeCategories.Count + expenseCategories.Count);
            importModel.State = Model.enImportState.Importing;
            await this.DataContext.SaveChangesAsync();

            // ACCOUNTS
            var accountList = await this.Import_Accounts(importModel, accounts);

            // CATEGORIES
            var incomeCategoriesList = await this.Import_Categories(importModel, incomeCategories, Categories.Model.enCategoryType.Income);
            var expenseCategoriesList = await this.Import_Categories(importModel, expenseCategories, Categories.Model.enCategoryType.Expense);

            // ENTRIES
            var entryList = await this.Import_Entries(importModel, dataList, accountList, incomeCategoriesList, expenseCategoriesList);

            // STATE
            importModel.State = Model.enImportState.Finished;
            await this.DataContext.SaveChangesAsync();

            return true;
         }
         catch (Exception ex) {
            importModel.Message += Environment.NewLine;
            importModel.Message += "Exception: " + ex.ToString();
            importModel.State = Model.enImportState.Canceled;
            await this.DataContext.SaveChangesAsync();
            return false;
         }
      }

      private List<Data> Import_GetData(Model.bindImport importModel,string filePath)
      {
         try
         {
            var resultList = new List<Data>();
            importModel.Message += "ReadingData: ";

            var incomeList = Data.GetData(importModel, filePath, Data.enDataType.Income);
            resultList.AddRange(incomeList);

            var expenseList = Data.GetData(importModel, filePath, Data.enDataType.Expense);
            resultList.AddRange(expenseList);

            var transferList = Data.GetData(importModel, filePath, Data.enDataType.Transfer);
            resultList.AddRange(transferList);

            if (resultList.Count != 0) { importModel.Message += "OK"; }
            importModel.Message += Environment.NewLine;
            return resultList;
         }
         catch (Exception ex) { throw ex; }
      }

      private async Task<List<Accounts.Model.viewAccount>> Import_Accounts(Model.bindImport importModel, List<string> accounts)
      {
         try
         {
            var resultList = new List<Accounts.Model.viewAccount>();
            importModel.Message += "ImportingAccounts: ";
            await this.DataContext.SaveChangesAsync();

            using (var apiController = new Accounts.AccountController())
            {
               // await accountController.RemoveUserData(this.GetUserID());

               foreach (var account in accounts)
               {
                  var apiModel = apiController.QueryView().Where(x => x.Text == account).FirstOrDefault();
                  if (apiModel == null)
                  {
                     apiController.ModelState.Clear();
                     var apiParam = new Accounts.Model.viewAccount() { Text = account, Type = Accounts.Model.enAccountType.General, Active = true };
                     var apiMessage = await apiController.Create(apiParam);
                     var apiResult = ((System.Web.Http.Results.CreatedNegotiatedContentResult<Accounts.Model.viewAccount>)apiMessage);
                     apiModel = apiResult.Content;
                  }
                  if (apiModel != null) { resultList.Add(apiModel); }
                  importModel.ImportedAccounts++;
                  await this.DataContext.SaveChangesAsync();
               }
            }

            importModel.Message += "OK" + Environment.NewLine;
            await this.DataContext.SaveChangesAsync();
            return resultList;
         }
         catch (Exception ex) { throw ex; }
      }

      private async Task<List<Categories.Model.viewCategory>> Import_Categories(Model.bindImport importModel, List<string> categories, Categories.Model.enCategoryType categoryType)
      {
         try
         {
            var resultList = new List<Categories.Model.viewCategory>();
            importModel.Message += "Importing" + categoryType.ToString() + "Categories: ";
            await this.DataContext.SaveChangesAsync();

            using (var categoryController = new Categories.CategoryController())
            {
               // await categoryController.RemoveUserData(this.GetUserID());

               foreach (var category in categories)
               {
                  var categoryModel = categoryController.QueryView().Where(x => x.Text == category).FirstOrDefault();
                  if (categoryModel == null)
                  {
                     categoryController.ModelState.Clear();
                     var categoryParam = new Categories.Model.viewCategory() { Text = category, Type = categoryType };
                     var categoryMessage = await categoryController.Create(categoryParam);
                     var categoryResult = ((System.Web.Http.Results.CreatedNegotiatedContentResult<Categories.Model.viewCategory>)categoryMessage);
                     categoryModel = categoryResult.Content;
                  }
                  if (categoryModel != null) { resultList.Add(categoryModel); }
                  importModel.ImportedCategories++;
                  await this.DataContext.SaveChangesAsync();
               }
            }

            importModel.Message += "OK" + Environment.NewLine;
            await this.DataContext.SaveChangesAsync();
            return resultList;
         }
         catch (Exception ex) { throw ex; }
      }

      private async Task<bool> Import_Entries(Model.bindImport importModel, List<Data> entries, List<Accounts.Model.viewAccount> accountList, List<Categories.Model.viewCategory> incomeCategoriesList, List<Categories.Model.viewCategory> expenseCategoriesList)
      {
         try
         {
            importModel.Message += "ImportingEntries: ";
            await this.DataContext.SaveChangesAsync();

            using (var entryController = new Entries.EntryController())
            {
               using (var transferController = new Entries.TransferController())
               {
                  // await accountController.RemoveUserData(this.GetUserID());

                  if (entries != null && entries.Count != 0)
                  {
                     foreach (var entry in entries)
                     {
                        if (entry.Description == null || string.IsNullOrEmpty(entry.Description)) { continue; }

                        // TRANSFER
                        if (entry.Type == Data.enDataType.Transfer)
                        { await this.Import_Entries_Transfer(importModel, transferController, entry, accountList, incomeCategoriesList, expenseCategoriesList); }

                        // ENTRY
                        else
                        { await this.Import_Entries_Entry(importModel, entryController, entry, accountList, incomeCategoriesList, expenseCategoriesList); }

                        importModel.ImportedEntries++;
                        await this.DataContext.SaveChangesAsync();
                     }
                  }

               }
            }

            importModel.Message += "OK" + Environment.NewLine;
            await this.DataContext.SaveChangesAsync();
            return true;
         }
         catch (Exception ex) { throw ex; }
      }

      private async Task<Entries.Model.viewEntry> Import_Entries_Entry(Model.bindImport importModel, Entries.EntryController apiController, Data entry, List<Accounts.Model.viewAccount> accountList, List<Categories.Model.viewCategory> incomeCategoriesList, List<Categories.Model.viewCategory> expenseCategoriesList)
      {
         try
         {

            // DATE
            var dateTimeArray = entry.Date.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (dateTimeArray != null && dateTimeArray.Length >= 1) {
               var dateArray = dateTimeArray[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
               if (dateArray != null && dateArray.Length == 3) {
                  var monthDate = int.Parse(dateArray[0]);
                  var dayDate = int.Parse(dateArray[1]);
                  var yearDate = int.Parse(dateArray[2]);
                  entry.Date = new DateTime(yearDate, monthDate, dayDate).ToString("dd/MM/yyyy");
               }
            }

            // PARAM
            var apiParam = new Entries.Model.viewEntry()
            {
               Text = entry.Description,
               DueDate = new DateTime(int.Parse(entry.Date.Substring(6, 4)), int.Parse(entry.Date.Substring(3, 2)), int.Parse(entry.Date.Substring(0, 2))), // dd/MM/yyyy
               EntryValue = Math.Round((double)entry.Value, 2)
            };

            // TYPE
            if (entry.Type == Data.enDataType.Income)
            {
               apiParam.Type = Entries.Model.enEntryType.Income;
               apiParam.idCategory = incomeCategoriesList.Where(x => x.Text == entry.Category).FirstOrDefault().idCategory;
            }
            else if (entry.Type == Data.enDataType.Expense)
            {
               apiParam.Type = Entries.Model.enEntryType.Expense;
               apiParam.idCategory = expenseCategoriesList.Where(x => x.Text == entry.Category).FirstOrDefault().idCategory;
            }

            // PAID
            if (entry.Paid.ToUpper() == "SIM")
            {
               apiParam.Paid = true;
               apiParam.PayDate = apiParam.DueDate;
               apiParam.idAccount = accountList.Where(x => x.Text == entry.Account).FirstOrDefault().idAccount;
            }

            // POST
            apiController.ModelState.Clear();
            var apiMessage = await apiController.Create(apiParam);
            var apiResult = ((System.Web.Http.Results.CreatedNegotiatedContentResult<Entries.Model.viewEntry>)apiMessage);
            return apiResult.Content;

         }
         catch (Exception ex)
         {
            importModel.Message += Environment.NewLine;
            importModel.Message += "Entry: " + FriendCash.Model.Base.Json.Serialize(entry);
            throw ex;
         }
      }

      private async Task<Entries.Model.viewTransfer> Import_Entries_Transfer(Model.bindImport importModel, Entries.TransferController apiController, Data entry, List<Accounts.Model.viewAccount> accountList, List<Categories.Model.viewCategory> incomeCategoriesList, List<Categories.Model.viewCategory> expenseCategoriesList)
      {
         try
         {

            // DATE
            var dateTimeArray = entry.Date.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (dateTimeArray != null && dateTimeArray.Length >= 1)
            {
               var dateArray = dateTimeArray[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
               if (dateArray != null && dateArray.Length == 3)
               {
                  var monthDate = int.Parse(dateArray[0]);
                  var dayDate = int.Parse(dateArray[1]);
                  var yearDate = int.Parse(dateArray[2]);
                  entry.Date = new DateTime(yearDate, monthDate, dayDate).ToString("dd/MM/yyyy");
               }
            }

            // PARAM
            var apiParam = new Entries.Model.viewTransfer()
            {
               Text = GetTranslation("ENUM_CATEGORYTYPE_TRANSFER"),
               DueDate = new DateTime(int.Parse(entry.Date.Substring(6, 4)), int.Parse(entry.Date.Substring(3, 2)), int.Parse(entry.Date.Substring(0, 2))), // dd/MM/yyyy
               Value = Math.Round((double)entry.Value, 2)
            };

            // PAID
            apiParam.Paid = true;
            apiParam.PayDate = apiParam.DueDate;

            // ACCOUNT
            apiParam.idAccountIncome = accountList.Where(x => x.Text == entry.AccountTo).FirstOrDefault().idAccount;
            apiParam.idAccountExpense = accountList.Where(x => x.Text == entry.AccountFrom).FirstOrDefault().idAccount;

            // POST
            apiController.ModelState.Clear();
            var apiMessage = await apiController.Create(apiParam);
            var apiResult = ((System.Web.Http.Results.CreatedNegotiatedContentResult<Entries.Model.viewTransfer>)apiMessage);
            return apiResult.Content;

         }
         catch (Exception ex)
         {
            importModel.Message += Environment.NewLine;
            importModel.Message += "Transfer: " + FriendCash.Model.Base.Json.Serialize(entry);
            throw ex;
         }
      }

      #endregion

   }

}
