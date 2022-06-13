using Lewio.CashFlow.Domains;
using Lewio.CashFlow.Services;

namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<DummyService>();

      serviceCollection
         .AddAccountsServices();

      return serviceCollection;
   }

}
