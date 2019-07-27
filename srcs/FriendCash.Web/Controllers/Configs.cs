#region Using
using FriendCash.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class ConfigsController : Controllers.Base
   {

      #region Index
      public ActionResult Index()
      {
         return View();
      }
      #endregion

      #region GetImportData
      public async Task<ActionResult> GetImportData(long id = 0)
      {

         // URL
         var apiURL = "api/import";
         if (id != 0) { apiURL += "\\" + id; }

         // CALL
         var bundleData = await Helper.nAPI<List<FriendCash.Service.Imports.Model.viewImport>>.GetAsync(this, apiURL);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region PostImportData

      public async Task<ActionResult> PostImportData()
      {

         // VALIDATE
         if (this.Request.Files.Count == 0 || this.Request.Files[0] == null || this.Request.Files[0].ContentLength == 0)
         { return null; }

         // BYTE ARRAY
         byte[] streamBytes = null;
         using (var binaryReader = new System.IO.BinaryReader(this.Request.Files[0].InputStream))
         { streamBytes = binaryReader.ReadBytes(this.Request.Files[0].ContentLength); }
         var byteArrayContent = new System.Net.Http.ByteArrayContent(streamBytes);

         // POST: URL
         var apiURL = "api/import";

         // POST: CALL
         // var bundleData = await this.API.PostAsync(apiURL, byteArrayContent);
         var bundleData = await this.PostImportData(apiURL, byteArrayContent);

         // POST: VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // POST: RESULT
         var bundleJson = Json(bundleData.Data.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }

      private async Task<Helper.ApiResult<System.Net.Http.HttpResponseMessage>> PostImportData(string sPath, System.Net.Http.ByteArrayContent oData)
      {
         using (var api = Helper.nAPI<System.Net.Http.HttpResponseMessage>.Load(this))
         {
            if (await api.ValidateToken() == false) { return api.Result; }
            if (api.ApplyAuthorization() == false) { return api.Result; }
            if (api.ApplyLanguages() == false) { return api.Result; }

            var apiMSG = await api.PostAsync(sPath, oData).ConfigureAwait(false);

            // RESULT
            var apiResult = new Helper.ApiResult<System.Net.Http.HttpResponseMessage>();
            apiResult.Data.Data = apiMSG;
            apiResult.Data.Result = true;
            apiResult.OK = true;
            return apiResult;
         }
      }

      #endregion

   }
}