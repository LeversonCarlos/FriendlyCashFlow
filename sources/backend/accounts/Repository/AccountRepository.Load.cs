using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public async Task<IAccountEntity> LoadAccountAsync(Shared.EntityID accountID) =>
         await _Collection
            .Find(account => account.AccountID == accountID)
            .SingleOrDefaultAsync();

   }
}
