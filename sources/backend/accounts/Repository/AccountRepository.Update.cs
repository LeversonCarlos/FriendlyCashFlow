using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task UpdateAccountAsync(IAccountEntity value) =>
         _Collection
            .ReplaceOneAsync(account => account.AccountID == value.AccountID, value as AccountEntity);

   }
}
