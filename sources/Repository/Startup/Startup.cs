using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.Repository;

public static class RepositoryInjector
{

   public static IServiceCollection InjectRepositories(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddSingleton<IMainRepository, MainRepository>()
         .AddSingleton<IAccountRepository, AccountRepository>();

      return serviceCollection;
   }
}
