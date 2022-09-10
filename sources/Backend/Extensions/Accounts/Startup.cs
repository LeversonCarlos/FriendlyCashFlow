using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow;

public static class AccountsStartupExtension
{

   public static IServiceCollection AddAccountsRepository(this IServiceCollection serviceCollection)
   {
      serviceCollection
         .AddScoped<IAccountRepository, AccountRepository>();
      return serviceCollection;
   }

   public static IServiceCollection AddAccountsCommands(this IServiceCollection serviceCollection)
   {
      serviceCollection
         .AddTransient<SearchCommand>()
         .AddTransient<LoadCommand>()
         .AddTransient<CreateCommand>()
         .AddTransient<UpdateCommand>()
         .AddTransient<DeleteCommand>();
      return serviceCollection;
   }

}
