using Microsoft.AspNetCore.Localization;
using System.Globalization;
namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureLocalization(this IServiceCollection serviceCollection)
   {
      serviceCollection
         .InjectLocalizationServices();

      return serviceCollection;
   }

   public static IApplicationBuilder ConfigureLocalization(this IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
   {

      var defaultCultureName = Shared.Localization.Resources.CultureInfo.DefaultCultureName;
      var supportedCultureNames = Shared.Localization.Resources.CultureInfo.SupportedCultureNames;

      var supportedCultures = supportedCultureNames
         .Select(name => new CultureInfo(name))
         .ToArray();

      applicationBuilder
         .UseRequestLocalization(new RequestLocalizationOptions
         {
            DefaultRequestCulture = new RequestCulture(defaultCultureName, defaultCultureName),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
            /*
            RequestCultureProviders = new List<IRequestCultureProvider>
            { new AcceptLanguageHeaderRequestCultureProvider () }
            */
         });

      return applicationBuilder;
   }

}
