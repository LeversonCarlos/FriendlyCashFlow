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
#endregion

namespace FriendCash.Web.Auth
{
   partial class AuthController
   {

      #region View
      [HttpGet]
      public ActionResult Login(string returnUrl)
      {

         // RETURN URL
         if (string.IsNullOrEmpty(returnUrl)) { returnUrl = Url.Action("Index", "Home"); }
         ViewBag.returnUrl = returnUrl;

         // USERNAME COOKIE
         if (this.Request.Cookies["UserName"] == null) { ViewBag.UserName = string.Empty; }
         else { ViewBag.UserName = this.Request.Cookies["UserName"].Value; }

         return View();
      }
      #endregion

      #region Post
      // [ValidateAntiForgeryToken]
      [HttpPost]
      public async Task<JsonResult> Login(string returnUrl, viewAuth Value) 
      {
         var oBundle = new Bundle<string>();
         try
         {
            
            // MODEL STATE
            if (!ModelState.IsValid)
            {
               var oModelState = new
               {
                  Message = this.GetTranslationByKey("MSG_AUTH_INVALID_LOGIN"),
                  ModelState = ModelState
                     .Where(x => x.Value.Errors.Count() != 0)
                     .Select(x => new KeyValuePair<string, List<string>>(x.Key, x.Value.Errors.Select(e => this.GetTranslationByKey(e.ErrorMessage)).ToList()))
                     //.ToDictionary(k => k.Key, k => k.Value, StringComparer.OrdinalIgnoreCase)
                     .ToList()
               };
               oBundle.Messages.Add(new BundleMessage(Helper.Json.SerializeObject(oModelState), BundleMessage.enumType.Information));
               return this.GetJsonResult(oBundle); 
            }

            // APPLY TOKEN
            var tokenApply = await this.TokenApply(Value.username, Value.password, Value.isPersistent, this.Authentication);
            if (tokenApply != null && !tokenApply.Result) { oBundle.Messages.AddRange(tokenApply.Messages); return this.GetJsonResult(oBundle); }

            // REDIRECT
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) { returnUrl = Url.Action("Index", "Home"); }
            oBundle.Data = returnUrl;
            oBundle.Result = true;

         }
         catch (Exception ex) { oBundle.Messages.Add(new BundleMessage(ex.ToString(), BundleMessage.enumType.Alert)); }
         return this.GetJsonResult(oBundle);
      }
      #endregion

      #region Logout
      [Authorize]
      [HttpGet]
      public ActionResult Logout()
      {
         try
         {

            // EXECUTE
            Authentication.SignOut("ApplicationCookie");
            // System.Web.Security.FormsAuthentication.SignOut();
            // HttpContext.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(string.Empty), null);
            // Session.Abandon();

            // REDIRECT
            return RedirectToAction("Index", "Home");

         }
         catch (Exception ex) { ModelState.AddModelError("ExceptionMessage", ex); return View("Error"); }
      }
      #endregion

   }
}