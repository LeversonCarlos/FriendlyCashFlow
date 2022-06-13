using Lewio.CashFlow.Domain;
using Lewio.CashFlow.Repository;
using Lewio.CashFlow.Services;

namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<DummyService>();

      serviceCollection
         .InjectRepositories()
         .InjectDomains();

      return serviceCollection;
   }

}
