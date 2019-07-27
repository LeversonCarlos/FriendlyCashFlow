#region Using
using FriendCash.Service.Entries.Model;
using FriendCash.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class EntriesController : Controllers.Base
   {
      private const string apiURL = "api/entries";
      private const string apiTransferURL = "api/transfers";

      #region Index
      public ActionResult Index(long? id)
      {
         long initialAccount = 0; if (id.HasValue) { initialAccount = id.Value; }
         ViewBag.initialAccount = initialAccount;
         return View();
      }
      #endregion

      #region GetInterval
      [HttpPost]
      public async Task<ActionResult> GetInterval(viewFilterParam value)
      {

         // URL
         var bundleUrl = string.Format(apiURL + "/interval/{0}/{1}/{2}/", value.Year, value.Month, value.Account);

         // CALL
         var bundleData = await Helper.nAPI<viewFilter>.GetAsync(this, bundleUrl);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion
      
      #region GetData
      public async Task<ActionResult> GetData(int year, int month, long account)
      {

         // URL
         var bundleUrl = string.Format("{0}/{1}/{2}", apiURL, year, month);
         if (account > 0) { bundleUrl += "/" + account; }

         // CALL
         var bundleData = await Helper.nAPI<List<viewEntry>>.GetAsync(this, bundleUrl);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region SaveData

      [HttpPost]
      public async Task<ActionResult> SaveData(viewEntry value)
      {

         // TRANSFER
         if (value.Type == enEntryType.None || !string.IsNullOrEmpty(value.idTransfer))
         { return await this.SaveData_Transfer(value); }

         // ENTRY
         else { return await this.SaveData_Entry(value); }

      }

      private async Task<ActionResult> SaveData_Entry(viewEntry value)
      {
         Helper.ApiResult<viewEntry> bundleData;

         // CALL
         if (value.idEntry <= 0) { bundleData = await Helper.nAPI<viewEntry>.PostAsync(this, apiURL, value); }
         else { bundleData = await Helper.nAPI<viewEntry>.PutAsync(this, apiURL, value); }

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }

      private async Task<ActionResult> SaveData_Transfer(viewEntry value)
      {
         Helper.ApiResult<viewTransfer> bundleData;

         // MODEL
         var transferValue = new viewTransfer()
         {
            idTransfer = value.idTransfer,
            Text =value.Text, 
            DueDate = value.DueDate,
            PayDate = value.DueDate,
            SearchDate = value.DueDate,
            idEntryExpense = value.idEntryExpense,
            idAccountExpense = value.idAccountExpense,
            idEntryIncome = value.idEntryIncome,
            idAccountIncome = value.idAccountIncome,
            Paid = true,
            Value = value.EntryValue
         };

         // CALL
         if (string.IsNullOrEmpty(transferValue.idTransfer)) { bundleData = await Helper.nAPI<viewTransfer>.PostAsync(this, apiTransferURL, transferValue); }
         else { bundleData = await Helper.nAPI<viewTransfer>.PutAsync(this, apiTransferURL, transferValue); }

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }

      #endregion

      #region RemoveData

      [HttpPost]
      public async Task<ActionResult> RemoveData(viewEntry value)
      {

         // TRANSFER
         if (!string.IsNullOrEmpty(value.idTransfer))
         { return await this.RemoveData_Transfer(value); }

         // ENTRY
         else { return await this.RemoveData_Entry(value); }

      }

      private async Task<ActionResult> RemoveData_Entry(viewEntry value)
      {

         // CALL
         var bundleData = await Helper.nAPI<viewEntry>.DeleteAsync(this, apiURL + "/" + value.idEntry);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }

      private async Task<ActionResult> RemoveData_Transfer(viewEntry value)
      {

         // CALL
         var bundleData = await Helper.nAPI<viewTransfer>.DeleteAsync(this, apiTransferURL + "/" + value.idTransfer);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }

      #endregion

   }
}