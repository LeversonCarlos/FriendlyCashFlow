using System.Threading.Tasks;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task InsertAccountAsync(IAccountEntity value) =>
         _Collection
            .InsertOneAsync(value as AccountEntity);

   }
}
