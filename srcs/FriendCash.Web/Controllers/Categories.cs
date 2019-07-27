#region Using
using FriendCash.Service.Categories.Model;
using FriendCash.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class CategoriesController : Controllers.Base
   {
      private const string apiURL = "api/categories";

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
         var bundleIncome = await Helper.nAPI<List<viewCategory>>.GetAsync(this, apiURL + "/" + enCategoryType.Income.ToString());
         var bundleExpense = await Helper.nAPI<List<viewCategory>>.GetAsync(this, apiURL + "/" + enCategoryType.Expense.ToString());

         // VALIDATE
         if (!bundleIncome.OK) { return new HttpStatusCodeResult(bundleIncome.StatusCode); }
         if (!bundleExpense.OK) { return new HttpStatusCodeResult(bundleExpense.StatusCode); }

         // RESULT
         var bundleData = new Bundle<dynamic>() { Data = new { Income = bundleIncome.Data.Data, Expense = bundleExpense.Data.Data }, Result = true };
         var bundleJson = Json(bundleData, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region GetRelated
      public async Task<ActionResult> GetRelated(long id = 0, string search = "", string filter = "")
      {

         // URL
         var type = Model.Base.Json.Deserialize<string>(filter);
         var relatedUrl = apiURL + "/" + type + "/" + "related" + "/";
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
      public async Task<ActionResult> SaveData(viewCategory Value)
      {
         Helper.ApiResult<viewCategory> bundleData;

         // CALL
         if (Value.idCategory <= 0) { bundleData = await Helper.nAPI<viewCategory>.PostAsync(this, apiURL, Value); }
         else { bundleData = await Helper.nAPI<viewCategory>.PutAsync(this, apiURL, Value); }

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region RemoveData
      [HttpPost]
      public async Task<ActionResult> RemoveData(viewCategory Value)
      {

         // CALL
         var bundleData = await Helper.nAPI<viewCategory>.DeleteAsync(this, apiURL + "/" + Value.idCategory);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

   }
}