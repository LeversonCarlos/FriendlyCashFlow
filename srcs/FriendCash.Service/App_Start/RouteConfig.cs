#region Using
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace FriendCash.Service.Configs
{
   public class Route
   {

      #region Register
      public static void Register(HttpConfiguration httpConfig)
      {
         Register_WebApi(httpConfig);
         // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
         // app.UseWebApi(httpConfig);
      }
      #endregion

      #region Register_WebApi
      private static void Register_WebApi(HttpConfiguration config)
      {

         // QUERYABLE RETURNING TYPES
         // config.EnableQuerySupport();

         // MODEL VALIDATION FILTER
         config.Filters.Add(new ValidateModelAttribute());

         // WEB API ROUTES
         config.MapHttpAttributeRoutes();
         /*
         config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
         */

         // JSON FORMATTER
         var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
         jsonFormatter.SerializerSettings.ContractResolver= new DefaultContractResolver(); //= new CamelCasePropertyNamesContractResolver();
      }
      #endregion

   }
}