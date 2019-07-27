#region Using
using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using FriendCash.Service.Base;
using FriendCash.Auth.Model;
#endregion

namespace FriendCash.Web.Auth
{
   partial class AuthController : Controllers.Base
   {

      #region View
      [HttpGet]
      public ActionResult Register()
      {
         return View();
      }
      #endregion

      #region Post
      // [ValidateAntiForgeryToken]
      [HttpPost]
      public async Task<JsonResult> Register(viewCreateUser value)
      {
         var oBundle = new Bundle<string>();
         try
         {

            // URL
            var apiUrl = "api/auth/user/create";

            // CALL
            var apiResult = await Helper.nAPI<viewUser>.PostAsync(this, apiUrl, value, true);
            if (!apiResult.Data.Result) { return this.GetJsonResult(apiResult.Data); }

            // RESULT
            oBundle.Data = Url.Action("RegisterResult", "Auth"); ;
            oBundle.Result = true;

         }
         catch (Exception ex) { oBundle.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return this.GetJsonResult(oBundle);
      }
      #endregion

      #region Result
      [HttpGet]
      public ActionResult RegisterResult()
      {
         return View();
      }
      #endregion



   }
}