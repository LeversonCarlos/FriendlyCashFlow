using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddLocalizationServices(IServiceCollection services)
      {
         services.AddLocalization(o => 
         {           
            o.ResourcesPath = "";
         });
      }

      private void UseLocalizationServices(IApplicationBuilder app, IHostingEnvironment env)
      {
         var supportedCultures = new List<CultureInfo>
         {
            new CultureInfo("en-US"),
            new CultureInfo("pt-BR")
         };
         app.UseRequestLocalization(new RequestLocalizationOptions
         {
            DefaultRequestCulture = new RequestCulture("en-US", "en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
            /*,
            RequestCultureProviders = new List<IRequestCultureProvider>
            {
               new AcceptLanguageHeaderRequestCultureProvider ()
            }
            */
         });
      }

   }
}
