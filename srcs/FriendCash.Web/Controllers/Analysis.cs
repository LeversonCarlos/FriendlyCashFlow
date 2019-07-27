#region Using
using FriendCash.Service.Analysis.Model;
using FriendCash.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class AnalysisController : Controllers.Base
   {
      private const string baseURL = "api/analysis";

      #region Index
      public ActionResult Index()
      {
         return View();
      }
      #endregion

      #region GetInterval
      [HttpPost]
      public async Task<ActionResult> GetInterval(viewParam value)
      {

         // URL
         var apiURL = string.Format(baseURL + "/interval/{0}/{1}", value.Year, value.Month);

         // CALL
         var bundleData = await Helper.nAPI<viewInterval>.GetAsync(this, apiURL);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region GetData
      public async Task<ActionResult> GetData(viewParam value)
      {

         // URL 
         var apiURL = string.Format(baseURL + "/data/{0}/{1}", value.Year, value.Month);

         // CALL
         var bundleData = await Helper.nAPI<viewData>.GetAsync(this, apiURL);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region GetGraphs
      public async Task<ActionResult> GetGraphs(viewData dataValue)
      {

         // URL
         var apiURL = string.Format(baseURL + "/graph");

         // CALL
         var bundleData = await Helper.nAPI<graphMain>.PostAsync(this, apiURL, dataValue);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

   }
}