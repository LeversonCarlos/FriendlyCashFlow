#region Using
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   public partial class Base : Controller
   {

      #region GetTranslationByKey

      internal string GetTranslationByKey(string key)
      {
         var resultJson = "";
         try
         {
            resultJson = Task.Run(() => this.GetTranslationByKeyAsync(key)).Result;
            return Helper.Json.DeserializeObject<string>(resultJson);
         }
         catch (Exception ex) { throw new Exception(resultJson + Environment.NewLine + ex.ToString()); }
      }

      internal async Task<string> GetTranslationByKeyAsync(string key)
      {

         // PARAMS
         var errorResult = "\"" + key + "\"";
         var bundleUrl = "api/translation/" + key + "";

         // CALL
         var bundleData = await Helper.nAPI<string>.GetAsync(this, bundleUrl, true);

         // RESULT
         if (!bundleData.OK) { return errorResult; }
         else if (bundleData.Data.Result) { return bundleData.Data.Data; }
         else { throw new Exception(bundleData.Data.Messages[0].Text); }

      }

      #endregion

      #region GetApiResult
      protected async Task<ActionResult> GetApiResult<T>(string apiURL)
      {
         try
         {

            // CALL
            var bundleResult = await Helper.nAPI<T>.GetAsync(this, apiURL);

            // VALIDATE
            if (bundleResult.OK) { return Json(bundleResult.Data, JsonRequestBehavior.AllowGet); }

            // ERRORS
            var jsonResult = FriendCash.Model.Base.Json.Serialize(bundleResult);
            switch (bundleResult.StatusCode)
            {

               case System.Net.HttpStatusCode.Unauthorized:
                  return new HttpUnauthorizedResult(jsonResult);

               case System.Net.HttpStatusCode.NotFound:
                  return new HttpNotFoundResult(jsonResult);

               default:
                  return new HttpStatusCodeResult(bundleResult.StatusCode, jsonResult);
            }


         }
         catch (Exception ex) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, ex.Message); }
      }
      #endregion

      #region GetJsonResult
      protected JsonResult GetJsonResult<T>(T value)
      { return Json(value, JsonRequestBehavior.AllowGet); }
      #endregion

   }

}