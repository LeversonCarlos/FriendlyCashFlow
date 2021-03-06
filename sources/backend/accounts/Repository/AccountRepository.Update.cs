using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task UpdateAsync(IAccountEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.AccountID == value.AccountID, value as AccountEntity);

   }
}
