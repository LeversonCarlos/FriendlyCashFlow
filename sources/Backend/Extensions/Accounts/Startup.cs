using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow;

public static class AccountsStartupExtension
{
   public static IServiceCollection AddAccountsServices(this IServiceCollection serviceCollection)
   {

      serviceCollection
         .AddScoped<IAccountRepository, AccountRepository>();

      serviceCollection
         .AddTransient<SearchCommand>();

      return serviceCollection;
   }
}
