using System.Threading.Tasks;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task InsertAsync(IAccountEntity value) =>
         _Collection
            .InsertOneAsync(value as AccountEntity);

   }
}
