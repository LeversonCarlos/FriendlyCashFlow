using Lewio.CashFlow.Domain.Accounts;

namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<IAccount> GetNew()
   {
      await Task.CompletedTask;
      var value = default(IAccount);
      return value!;
   }

}
