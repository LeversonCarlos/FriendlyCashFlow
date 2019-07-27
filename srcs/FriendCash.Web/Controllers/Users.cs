#region Using
using FriendCash.Auth.Model;
using FriendCash.Service.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class UsersController : Controllers.Base
   {
      private const string apiURL = "api/auth";

      #region Index
      public ActionResult Index()
      {
         return View();
      }
      #endregion

      #region Details
      public ActionResult Details(string id)
      {
         ViewBag.userID = id;
         return View();
      }
      #endregion

      #region GetData

      public async Task<ActionResult> GetData(string id)
      {

         // LIST OF USERS
         if (string.IsNullOrEmpty(id))
         {
            var urlData = string.Format("{0}/users", apiURL);
            var bundleJson = await GetData<List<viewUser>>(urlData);
            return bundleJson;
         }

         // SPECIFIC USER
         else
         {
            var urlData = string.Format("{0}/user/{1}", apiURL, id);
            var bundleJson = await GetData<viewUser>(urlData);
            return bundleJson;
         }

      }

      private async Task<ActionResult> GetData<T>(string urlData)
      {

         // CALL
         var bundleData = await Helper.nAPI<T>.GetAsync(this, urlData);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }

      #endregion

      #region GetSignatures
      public async Task<ActionResult> GetSignatures(string id)
      {

         // CALL 
         var urlData = string.Format("{0}/signatures/{1}", apiURL, id);
         var bundleData = await Helper.nAPI<List<viewSignature>>.GetAsync(this, urlData);

         // VALIDATE
         if (!bundleData.OK) { return new HttpStatusCodeResult(bundleData.StatusCode); }

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region AddSignature
      public async Task<ActionResult> AddSignature(editSignature value)
      {
         // CALL 
         var urlData = string.Format("{0}/signature", apiURL);
         var bundleData = await Helper.nAPI<viewSignature>.PostAsync(this, urlData, value);

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

      #region RemoveSignature
      public async Task<ActionResult> RemoveSignature(editSignature value)
      {
         // CALL 
         var urlData = string.Format("{0}/signature/{1}/{2}", apiURL, value.idUser, value.idSignature);
         var bundleData = await Helper.nAPI<viewSignature>.DeleteAsync(this, urlData);

         // RESULT
         var bundleJson = Json(bundleData.Data, JsonRequestBehavior.AllowGet);
         return bundleJson;

      }
      #endregion

   }
}