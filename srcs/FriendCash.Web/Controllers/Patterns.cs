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
   public class PatternsController : Controllers.Base
   {
      private const string apiURL = "api/pattern";

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

   }
}