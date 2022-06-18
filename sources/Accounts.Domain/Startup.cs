using Lewio.CashFlow.Accounts;
using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow;

public static class AccountsServiceCollection
{

   public static IServiceCollection InjectAccountDomain(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<SaveCommand>();

      return serviceCollection;
   }
}
