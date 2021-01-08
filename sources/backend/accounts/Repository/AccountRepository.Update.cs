using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public async Task UpdateAccountAsync(IAccountEntity value) =>
         await _Collection
            .ReplaceOneAsync(account => account.AccountID == value.AccountID, value as AccountEntity);

   }
}
