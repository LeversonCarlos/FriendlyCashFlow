using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<IAccountEntity> GetNew()
   {
      await Task.CompletedTask;
      var value = default(IAccountEntity);
      return value!;
   }

}
