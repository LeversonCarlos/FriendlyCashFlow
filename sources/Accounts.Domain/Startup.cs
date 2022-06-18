using Lewio.CashFlow.Accounts;
using Microsoft.Extensions.DependencyInjection;
namespace Lewio.CashFlow;

public static class AccountsServiceCollection
{

   public static IServiceCollection InjectAccountDomain(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<SearchCommand>()
         .AddScoped<LoadCommand>()
         .AddScoped<SaveCommand>()
         .AddScoped<RemoveCommand>();

      return serviceCollection;
   }
}
