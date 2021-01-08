using System.Threading.Tasks;
using Elesse.Shared;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public Task<IAccountEntity[]> ListAccountsAsync() =>
         throw new System.NotImplementedException();

   }
}
