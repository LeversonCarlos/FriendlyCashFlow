using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<IAccountEntity> GetByID(Guid id)
   {
      await Task.CompletedTask;
      var value = default(IAccountEntity);
      return value!;
   }

}
