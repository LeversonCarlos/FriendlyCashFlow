using Lewio.CashFlow.Accounts;

namespace Lewio.CashFlow
{

   namespace Accounts
   {
      internal partial interface IAccountRepository { }
      internal partial class AccountRepository : BaseRepository, IAccountRepository
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

