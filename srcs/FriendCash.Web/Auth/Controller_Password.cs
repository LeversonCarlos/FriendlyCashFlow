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
      [Authorize]
      [HttpGet]
      public ActionResult Password()
      {
         return  PartialView("~/Views/Auth/Password");
      }
      #endregion

      #region Post
      // [ValidateAntiForgeryToken]
      [Authorize]
      [HttpPost]
      public async Task<JsonResult> Password(viewChangePassword value)
      {
         var oBundle = new Bundle<string>();
         try
         {

            // URL
            var apiUrl = "api/auth/ChangePassword";

            // CALL
            var apiResult = await Helper.nAPI<bool>.PostAsync(this, apiUrl, value);
            if (!apiResult.Data.Result) { return this.GetJsonResult(apiResult.Data); }

            // RESULT
            oBundle.Data = Url.Action("Index", "Home"); ;
            oBundle.Result = true;

         }
         catch (Exception ex) { oBundle.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return this.GetJsonResult(oBundle);
      }
      #endregion

   }
}