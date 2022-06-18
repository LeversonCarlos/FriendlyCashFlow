using Lewio.CashFlow.Domain.Accounts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Domain;

public static class AccountsServiceCollection
{

   internal static IServiceCollection InjectAccountServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<SaveService>();

      return serviceCollection;
   }
}
