using Microsoft.EntityFrameworkCore;
namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   Task<AccountEntity?> GetByID(string id);
}

partial class AccountRepository
{

   public async Task<AccountEntity?> GetByID(string id)
   {

      var query = _DataContext.Accounts
         .WithLoggedInUser(GetLoggedInUser())
         .Where(x => x.AccountID == id);

      var data = await query.FirstOrDefaultAsync();

      return data;
   }

}
