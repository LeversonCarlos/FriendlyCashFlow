using System.Globalization;
using Lewio.CashFlow.Shared;
using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow;

public static class LocalizationServiceCollection
{

   public static IServiceCollection InjectLocalizationServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddSingleton<ILocalization, Localizator>()
         .AddLocalization(options =>
         {
            options.ResourcesPath = "";
         });

      return serviceCollection;
   }

}
