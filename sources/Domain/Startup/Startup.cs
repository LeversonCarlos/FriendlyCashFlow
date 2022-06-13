using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Domains;

public static class DomainInjector
{

   public static IServiceCollection InjectDomains(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .InjectAccountServices();

      return serviceCollection;
   }
}
