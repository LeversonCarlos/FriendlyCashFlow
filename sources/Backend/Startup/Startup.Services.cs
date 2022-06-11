using Friendly.CashFlow.Services;

namespace Friendly.CashFlow;

partial class StartupExtensions
{

   public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<DummyService>();

      return serviceCollection;
   }

}
