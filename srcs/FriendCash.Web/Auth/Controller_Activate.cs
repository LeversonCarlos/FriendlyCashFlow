#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using FriendCash.Web.Auth.Model;
using Microsoft.Owin.Security;
using System.Security.Claims;
using FriendCash.Service.Base;
using FriendCash.Auth.Model;
#endregion

namespace FriendCash.Web.Auth
{
   partial class AuthController
   {

      #region View

      [HttpGet]
      public async Task<ActionResult> Activate(string id, string code)
      {
         ViewBag.ApiResult = await this.Activate_API(id, System.Uri.EscapeDataString(code));
         return View("RegisterActivate");
      }

      private async Task<Bundle<bool>> Activate_API(string id, string code)
      {
         try
         {

            // URL
            var apiUrl = string.Format("api/auth/CreateConfirm?id={0}&code={1}", id, code);

            // CALL
            var apiResult = await Helper.nAPI<bool>.GetAsync(this, apiUrl, true);
            return apiResult.Data;

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

   }
}