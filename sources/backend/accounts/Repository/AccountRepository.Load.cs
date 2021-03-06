using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public async Task<IAccountEntity> LoadAsync(Shared.EntityID accountID) =>
         await _Collection
            .Find(entity => entity.AccountID == accountID)
            .SingleOrDefaultAsync();

   }
}
