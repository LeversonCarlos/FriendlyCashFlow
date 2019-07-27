#region Using
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Web
{
   partial class Startup
   {

      #region ConfigureAuth
      internal void ConfigureAuth(IAppBuilder app)
      {
         this.ConfigureAuth_Cookie(app);
      }
      #endregion

      #region ConfigureAuth_Cookie
      private void ConfigureAuth_Cookie(IAppBuilder app)
      {
         app.UseCookieAuthentication(new CookieAuthenticationOptions
         {
            SlidingExpiration = false,
            AuthenticationType = "ApplicationCookie",
            CookieName = System.Web.Security.FormsAuthentication.FormsCookieName,
            AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
            LoginPath = new PathString("/auth/login"),
            // LogoutPath = new PathString("/auth/logout"),
            Provider = new CookieAuthenticationProvider
            {
               /*
               // Enables the application to validate the security stamp when the user logs in.
               // This is a security feature which is used when you change a password or add an external login to your account.  
               OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                   validateInterval: TimeSpan.FromMinutes(30),
                   regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
               */
            }
         });
      }
      #endregion 

   }
}