#region Using
using FriendCash.Service.Accounts.Model;
using FriendCash.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class AccountsController : Controllers.Base
   {
      private const string apiURL = "api/accounts";

      #region Index
      public ActionResult Index()
      {
         return View();
      }
      #endregion

      #region GetData
      public async Task<ActionResult> GetData()
      { 

         // CALL
         var bundleData = await Helper.nAPI<List<viewAccount>>.GetAsync(this, apiURL);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region GetRelated
      public async Task<ActionResult> GetRelated(long id = 0, string search = "")
      {

         // URL
         var relatedUrl = apiURL + "/" + "related" + "/";
         if (id > 0) { relatedUrl += id; }
         if (!string.IsNullOrEmpty(search)) { relatedUrl += System.Uri.EscapeDataString(search); }

         // CALL
         var bundleData = await Helper.nAPI<List<viewRelated>>.GetAsync(this, relatedUrl);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region SaveData
      [HttpPost]
      public async Task<ActionResult> SaveData(viewAccount Value)
      {
         Helper.ApiResult<viewAccount> bundleData;

         // CALL
         if (Value.idAccount <= 0) { bundleData = await Helper.nAPI<viewAccount>.PostAsync(this, apiURL, Value); }
         else { bundleData = await Helper.nAPI<viewAccount>.PutAsync(this, apiURL, Value); }

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region RemoveData
      [HttpPost]
      public async Task<ActionResult> RemoveData(viewAccount Value)
      {

         // CALL
         var bundleData = await Helper.nAPI<viewAccount>.DeleteAsync(this, apiURL + "/" + Value.idAccount);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

   }
}