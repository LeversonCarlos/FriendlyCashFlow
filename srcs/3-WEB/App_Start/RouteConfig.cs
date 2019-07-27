using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Index",
               url: "{controller}/Index/{page}/{search}",
               defaults: new
               {
                  controller = "Home",
                  action = "Index",
                  page = UrlParameter.Optional,
                  search = UrlParameter.Optional
               }
            );

            routes.MapRoute(
               name: "History",
               url: "{controller}/History/{id}/{page}",
               defaults: new
                {
                   controller = "Home",
                   action = "History",
                   id = UrlParameter.Optional,
                   page = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}