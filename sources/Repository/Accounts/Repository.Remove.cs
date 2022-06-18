using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<bool> Remove(IAccountEntity value)
   {
      await Task.CompletedTask;
      return true;
   }

}
