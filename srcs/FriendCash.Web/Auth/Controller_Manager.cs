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

      #region DeleteUserData
      [Authorize]
      [HttpPost]
      public async Task<JsonResult> DeleteUserData()
      {
         var oBundle = new Bundle<string>();
         try
         {

            // URL
            var apiUrl = "api/auth/deleteUserData";

            // CALL
            var apiResult = await Helper.nAPI<bool>.DeleteAsync(this, apiUrl);
            if (!apiResult.Data.Result) { return this.GetJsonResult(apiResult.Data); }

            // RESULT
            oBundle.Data = Url.Action("Index", "Home"); ;
            oBundle.Result = true;

         }
         catch (Exception ex) { oBundle.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return this.GetJsonResult(oBundle);
      }
      #endregion

      #region CheckToken

      [Authorize]
      [HttpGet]
      public async Task<JsonResult> CheckToken()
      {
         var bundleResult = new Bundle<CheckTokenModel>();
         bundleResult.Data = new CheckTokenModel();
         try
         {

            // API CALL TEST
            var apiUrl = string.Format("api/auth/user/{0}/", this.User.Identity.Name);
            var apiResult = await Helper.nAPI<viewUser>.GetAsync(this, apiUrl);

            // NEXT CYCLE
            if (apiResult.Data.Result) {
               bundleResult.Data.accessDate = this.UserTicket.AccessExpiration;
               bundleResult.Data.nextCycle = this.UserTicket.AccessExpirationSeconds;
               bundleResult.Data.refreshDate = this.UserTicket.RefreshExpiration;
               bundleResult.Data.UserTicket = this.UserTicket;
            }
            else {
               bundleResult.Data.nextCycle = 1;
            }
            bundleResult.Result = true;

         }
         catch (Exception ex) { bundleResult.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return this.GetJsonResult(bundleResult);
      }

      internal class CheckTokenModel {
         public long nextCycle { get; set; }
         internal DateTime accessDate { get; set; }
         public string access { get { return accessDate.ToString("yyyy-MM-dd HH:mm:ss"); } }
         internal DateTime refreshDate { get; set; }
         public string refresh { get { return refreshDate.ToString("yyyy-MM-dd HH:mm:ss"); } }
         public Controllers.UserTicket UserTicket { get; set; }
      }

      #endregion

   }
}