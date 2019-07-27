using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using FriendCash.Model.Membership;

namespace FriendCash.Web.Controllers
{
   [Authorize]
   public class UserController : MasterController
   {

      #region New

      public UserController()
         : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
      { }

      public UserController(UserManager<ApplicationUser> userManager)
      { 
         this.UserManager = userManager;
         this.PageTitle = Resources.User.PAGE_TITLE_AUTHENTICATION;
       }

      #endregion

      #region Properties

      #region AuthenticationManager
      private IAuthenticationManager AuthenticationManager
      {
         get { return HttpContext.GetOwinContext().Authentication; }
      }
      #endregion

      #region UserManager
      public UserManager<ApplicationUser> UserManager { get; private set; }
      #endregion

      #endregion

      #region Actions

      #region Login

      [AllowAnonymous]
      public ActionResult Login(string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Login(FriendCash.Model.Membership.LocalLogin model, string returnUrl)
      {
         try
         {
            if (ModelState.IsValid)
            {
               var oUser = await UserManager.FindAsync(model.UserName, model.Password);
               if (oUser != null)
               {
                  await this.SignInAsync(oUser, model.RememberMe);
                  return RedirectToLocal(returnUrl);
                }
               this.AddMessageWarning("The user name or password provided is incorrect.");
             }
          }
         catch (Exception ex) { this.AddMessageException(ex.Message); }

         return View(model);
      }

      #endregion

      #region Register

      [AllowAnonymous]
      public ActionResult Register()
      {
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Register(FriendCash.Model.Membership.LocalRegister model)
      {
         try
         {
            if (ModelState.IsValid)
            {
               var oUser = new ApplicationUser() { UserName = model.UserName };
               var oCreateResult = await UserManager.CreateAsync(oUser, model.Password);
               if (oCreateResult.Succeeded)
               {
                  await this.SignInAsync(oUser, isPersistent: false);
                  return RedirectToAction("Index", "Home");
               }
               else { this.AddIdentityErrors(oCreateResult); }
             }
          }
         catch (Exception ex) { this.AddMessageException(ex.Message); }

         return View(model);
      }

      #endregion

      #region Manage

      public ActionResult Manage(ManageMessageId? message)
      {
         ViewBag.UserManageMessage =
             message == ManageMessageId.ChangePasswordSuccess ? Resources.User.MSG_INFO_CHANGE_PASSWORD_SUCCESS
             : message == ManageMessageId.SetPasswordSuccess ? Resources.User.MSG_INFO_SET_PASSWORD_SUCCESS 
             : message == ManageMessageId.RemoveLoginSuccess ? Resources.User.MSG_INFO_SERVICE_LOGIN_REMOVED
             : "";
         ViewBag.HasLocalPassword = this.HasLocalPassword();
         ViewBag.ReturnUrl = Url.Action("Manage");
         this.PageTitle = Resources.User.PAGE_TITLE_PROFILE;
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Manage(FriendCash.Model.Membership.LocalPassword model)
      {
         bool hasLocalPassword = this.HasLocalPassword();
         ViewBag.HasLocalPassword = hasLocalPassword;
         ViewBag.ReturnUrl = Url.Action("Manage");
         if (hasLocalPassword)
         {
            if (ModelState.IsValid)
            {
               IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
               if (result.Succeeded) { return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess }); }
               else { this.AddIdentityErrors(result); }
            }
         }
         else
         {
            // User does not have a password so remove any validation errors caused by a missing OldPassword field
            ModelState state = ModelState["OldPassword"];
            if (state != null) { state.Errors.Clear(); }

            if (ModelState.IsValid)
            {
               IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
               if (result.Succeeded) { return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess }); }
               else { this.AddIdentityErrors(result); }
            }
         }

         return View(model);
      }

      #endregion

      #region LogOff

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LogOff()
      {
         this.AuthenticationManager.SignOut();
         this.Session.Clear();
         return RedirectToAction("Index", "Home");
      }

      #endregion

      #region Service

      #region List
      [AllowAnonymous]
      [ChildActionOnly]
      public ActionResult ServiceList(string returnUrl)
      {
         object linkedAccounts = null;
         var UserID = User.Identity.GetUserId();
         if (UserID != null && !string.IsNullOrEmpty(UserID)) 
         {
            try { linkedAccounts = UserManager.GetLogins(UserID); } catch { }
          }
         ViewBag.ReturnUrl = returnUrl;
         ViewBag.ShowRemoveButton = this.HasLocalPassword(); //|| linkedAccounts.Count > 1;
         return (ActionResult)PartialView("ServiceList", linkedAccounts);
      }
      #endregion

      #region Login

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult ServiceLogin(string provider, string returnUrl)
      {
         return new ChallengeResult(provider, Url.Action("ServiceCallback", new { ReturnUrl = returnUrl }));
      }

      [AllowAnonymous]
      public async Task<ActionResult> ServiceCallback(string returnUrl)
      {
         try
         {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) { return RedirectToAction("Login"); }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
               await this.SignInAsync(user, isPersistent: false);
               return RedirectToLocal(returnUrl);
            }
            else
            {
               // If the user does not have an account, then prompt the user to create an account
               ViewBag.ReturnUrl = returnUrl;
               ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
               return View("ServiceConfirmation", new FriendCash.Model.Membership.ExternalRegister { UserName = loginInfo.DefaultUserName, ExternalLoginData = string.Empty });
            }
         }
         catch (Exception ex) 
         { 
            this.AddMessageException(ex.Message); 
            return RedirectToAction("Login"); 
         }
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ServiceConfirmation(FriendCash.Model.Membership.ExternalRegister model, string returnUrl)
      {
         try
         {
            if (User.Identity.IsAuthenticated) { return RedirectToAction("Manage"); }
            if (ModelState.IsValid)
            {
               // Get the information about the user from the external login provider
               var info = await AuthenticationManager.GetExternalLoginInfoAsync();
               if (info == null) { return View("ServiceLoginFailure"); }

               var user = new ApplicationUser() { UserName = model.UserName };
               var result = await UserManager.CreateAsync(user);
               if (result.Succeeded)
               {
                  result = await UserManager.AddLoginAsync(user.Id, info.Login);
                  if (result.Succeeded)
                  {
                     await this.SignInAsync(user, isPersistent: false);
                     return RedirectToLocal(returnUrl);
                  }
               }
               else { this.AddIdentityErrors(result); }
             }
            ViewBag.ReturnUrl = returnUrl;
         }
         catch (Exception ex) { this.AddMessageException(ex.Message); }

         return View(model);
      }

      [AllowAnonymous]
      public ActionResult ServiceLoginFailure()
      {
         return View();
      }

      #endregion

      #region Link

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LinkLogin(string provider)
      {
         // Request a redirect to the external login provider to link a login for the current user
         return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
      }

      public async Task<ActionResult> LinkLoginCallback()
      {
         var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
         if (loginInfo == null)
         {
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
         }
         var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
         if (result.Succeeded)
         {
            return RedirectToAction("Manage");
         }
         return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
      }

      #endregion

      #region Remove
      [ChildActionOnly]
      public ActionResult ServiceRemove()
      {
         var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
         ViewBag.ShowRemoveButton = this.HasLocalPassword() || linkedAccounts.Count > 1;
         return (ActionResult)PartialView("ServiceRemove", linkedAccounts);
      }
      #endregion

      #region Disassociate

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ServiceDisassociate(string loginProvider, string providerKey)
      {
         ManageMessageId? message = null;

         try
         {

            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded) { message = ManageMessageId.RemoveLoginSuccess; }
            else { message = ManageMessageId.Error; }

          }
         catch (Exception ex) { this.AddMessageException(ex.Message); }

         return RedirectToAction("Manage", new { Message = message });
      }

      #endregion

      #endregion

      #region Helpers

      private const string XsrfKey = "XsrfId";
      public enum ManageMessageId { ChangePasswordSuccess, SetPasswordSuccess, RemoveLoginSuccess, Error }

      #region SignInAsync

      private async Task SignInAsync(ApplicationUser oUser, bool isPersistent)
      {
         try
         {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(oUser, DefaultAuthenticationTypes.ApplicationCookie);
            this.AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
         }
         catch { throw; }
      }

      private bool SignInAsync_RegisterLogin(string sUserName)
      {
         bool bReturn = false;

         try
         {

            this.MyLogin = this.GetRegisteredLogin(sUserName, true);
            if (this.MyLogin != null)
            { bReturn = true; }

         }
         catch (Exception) { throw; }

         return bReturn;
      }

      #endregion

      #region AddIdentityErrors
      private void AddIdentityErrors(IdentityResult oResult)
      {
         foreach (var error in oResult.Errors)
         {
            this.AddMessageWarning(error);
         }
      }
      #endregion

      #region HasLocalPassword
      private bool HasLocalPassword()
      {
         var oUser = UserManager.FindById(User.Identity.GetUserId());
         if (oUser != null) { return oUser.PasswordHash != null; }
         return false;
      }
      #endregion

      #region RedirectToLocal
      private ActionResult RedirectToLocal(string returnUrl)
      {
         if (Url.IsLocalUrl(returnUrl))
         {
            return Redirect(returnUrl);
         }
         else
         {
            return RedirectToAction("Index", "Home");
         }
      }
      #endregion

      #region ChallengeResult
      internal class ChallengeResult : HttpUnauthorizedResult
      {

         #region New

         public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
         { }

         public ChallengeResult(string provider, string redirectUri, string userId)
         {
            this.LoginProvider = provider;
            this.RedirectUri = redirectUri;
            this.UserId = userId;
         }

         #endregion

         #region Properties

         public string LoginProvider { get; set; }
         public string RedirectUri { get; set; }
         public string UserId { get; set; }

         #endregion

         #region ExecuteResult

         public override void ExecuteResult(ControllerContext context)
         {
            var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
            if (UserId != null)
            {
               properties.Dictionary[XsrfKey] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
         }

         #endregion

      }

      #endregion

      #endregion
      
      #endregion

      #region Dispose
      protected override void Dispose(bool disposing)
      {
         if (disposing && UserManager != null)
         {
            UserManager.Dispose();
            UserManager = null;
         }
         base.Dispose(disposing);
      }
      #endregion

   }
}
