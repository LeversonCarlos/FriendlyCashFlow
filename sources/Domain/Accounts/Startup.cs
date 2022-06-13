using Lewio.CashFlow.Domains.Accounts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Domains;

public static class AccountsServiceCollection
{

   internal static IServiceCollection InjectAccountServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<CreateService>();

      return serviceCollection;
   }
}
