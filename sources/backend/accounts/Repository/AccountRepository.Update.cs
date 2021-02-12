using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task UpdateAccountAsync(IAccountEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.AccountID == value.AccountID, value as AccountEntity);

   }
}
