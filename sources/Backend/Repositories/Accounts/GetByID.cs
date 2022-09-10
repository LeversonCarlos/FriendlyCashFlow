using Microsoft.EntityFrameworkCore;
namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   Task<AccountEntity?> GetByID(EntityID id);
}

partial class AccountRepository
{

   public async Task<AccountEntity?> GetByID(EntityID id)
   {

      var query = _DataContext.Accounts
         .WithLoggedInUser(GetLoggedInUser())
         .Where(x => x.AccountID == id && x.RowStatus == 1);

      var data = await query.FirstOrDefaultAsync();

      return data;
   }

}
