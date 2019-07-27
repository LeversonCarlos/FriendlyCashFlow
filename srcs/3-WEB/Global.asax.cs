using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
   // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
   // visit http://go.microsoft.com/?LinkId=9394801

   public class MvcApplication : System.Web.HttpApplication
   {
      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute(
             "Index",
             "{controller}/Index/{page}/{search}",
             new
             {
                controller = "Home",
                action = "Index",
                page = UrlParameter.Optional,
                search = UrlParameter.Optional
             }
         );

         routes.MapRoute(
             "History",
             "{controller}/History/{id}/{page}",
             new
             {
                controller = "Home",
                action = "History",
                id = UrlParameter.Optional,
                page = UrlParameter.Optional
             }
         );

         routes.MapRoute(
             "Default", 
             "{controller}/{action}/{id}", 
             new 
             { 
                controller = "Home", 
                action = "Index", 
                id = UrlParameter.Optional 
              } 
         );

      }

      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();

         RegisterRoutes(RouteTable.Routes);
      }
   }
}