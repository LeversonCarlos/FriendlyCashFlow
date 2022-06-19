using Lewio.CashFlow.Repository;

namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<DummyService>();

      serviceCollection
         .InjectLocalizationServices()
         .InjectRepositories()
         .InjectAccountDomain();

      return serviceCollection;
   }

}
