using Lewio.CashFlow.Accounts;

namespace Lewio.CashFlow
{

   namespace Accounts
   {
      public partial interface IAccountRepository { }
      public partial class AccountRepository : BaseRepository, IAccountRepository
      {
         public AccountRepository(IServiceProvider serviceProvider) : base(serviceProvider) { }
      }
   }

   namespace Common
   {
      partial interface IMainRepository
      {
         IAccountRepository Accounts { get; }
      }
      partial class MainRepository
      {
         public IAccountRepository Accounts { get => _ServiceProvider.GetRequiredService<IAccountRepository>(); }
      }
   }

}

