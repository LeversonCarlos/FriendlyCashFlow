namespace Lewio.CashFlow.Repository;

public partial class AccountRepository : IAccountRepository
{

   public AccountRepository(IServiceProvider serviceProvider) =>
      _ServiceProvider = serviceProvider;
   protected readonly IServiceProvider _ServiceProvider;

   public virtual void Dispose()
   {
   }

}
