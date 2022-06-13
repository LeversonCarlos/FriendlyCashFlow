using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Repository;

public static class AccountsRepositoryCollection
{

   public static IServiceCollection AddAccountRepository(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddSingleton<IAccountRepository, AccountRepository>();

      return serviceCollection;
   }
}
