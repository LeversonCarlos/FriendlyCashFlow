using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<bool> SaveNew(IAccount value)
   {
      await Task.CompletedTask;
      return true;
   }

}
