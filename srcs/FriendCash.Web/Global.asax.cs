#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
#endregion

namespace FriendCash.Web
{
   public class Global : System.Web.HttpApplication
   {

      protected void Application_Start(object sender, EventArgs e)
      {
         // AreaRegistration.RegisterAllAreas();
         // FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         // GlobalConfiguration.Configure(FriendCash.Service.Configs.Route.Register);
         Configs.Route.Register(RouteTable.Routes);
         // BundleConfig.RegisterBundles(BundleTable.Bundles);
      }

      /*
      protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
      {
         var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
         if (authCookie != null)
         {
            //var userPrincipal = Auth.Cookies.SetCookie(authCookie);
            //HttpContext.Current.User = userPrincipal;
         }
       }
      */ 

      /*
       
      protected void Session_Start(object sender, EventArgs e) { }

      protected void Application_BeginRequest(object sender, EventArgs e) { }

      protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

      protected void Application_Error(object sender, EventArgs e) { }

      protected void Session_End(object sender, EventArgs e) { }

      protected void Application_End(object sender, EventArgs e) { }
       
      */ 

   }
}