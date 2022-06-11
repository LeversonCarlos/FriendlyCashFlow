using Lewio.CashFlow.Services;

namespace Lewio.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<DummyService>();

      return serviceCollection;
   }

}
