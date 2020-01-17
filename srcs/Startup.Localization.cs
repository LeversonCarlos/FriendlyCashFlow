using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddLocalization(IServiceCollection services)
      {
         services.AddLocalization(o => 
         {           
            o.ResourcesPath = "";
         });
      }

      private void UseLocalization(IApplicationBuilder app, IWebHostEnvironment env)
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
            /*
            RequestCultureProviders = new List<IRequestCultureProvider>
            { new AcceptLanguageHeaderRequestCultureProvider () }
            */
         });
      }

   }
}
