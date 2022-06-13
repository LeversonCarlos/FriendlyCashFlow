using Lewio.CashFlow.Domains.Accounts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Domains;

public static class AccountsServiceCollection
{

   public static IServiceCollection AddAccountsServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<CreateService>();

      return serviceCollection;
   }
}
