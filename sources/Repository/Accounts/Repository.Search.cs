using Lewio.CashFlow.Accounts;
namespace Lewio.CashFlow.Repository;

partial class AccountRepository
{

   public async Task<IAccountEntity[]> Search(string searchTerms)
   {
      await Task.CompletedTask;
      var value = default(IAccountEntity);
      return new[] { value! };
   }

}
