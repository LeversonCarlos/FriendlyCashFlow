using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Domain;

public static class DomainInjector
{

   public static IServiceCollection InjectDomains(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .InjectAccountServices();

      return serviceCollection;
   }
}
