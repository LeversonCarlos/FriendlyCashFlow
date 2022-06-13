using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<AccountEntity> SaveNew(AccountEntity value)
   {
      await Task.CompletedTask;
      return value;
   }

}
