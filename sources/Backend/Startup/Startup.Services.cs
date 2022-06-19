using System.Globalization;
using Lewio.CashFlow.Repository;
using Microsoft.AspNetCore.Localization;

namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<DummyService>();

      serviceCollection
         .InjectRepositories()
         .InjectAccountDomain();

      return serviceCollection;
   }

}
