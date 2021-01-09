using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task DeleteAccountAsync(Shared.EntityID accountID) =>
         _Collection
            .UpdateOneAsync(account => account.AccountID == accountID, Builders<AccountEntity>.Update.Set(x => x.RowStatus, false));

   }
}