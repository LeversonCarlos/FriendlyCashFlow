using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<ActionResult<IAccountEntity>> LoadAsync(Shared.EntityID accountID)
      {

         // LOAD ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAccountAsync(accountID));

         // RESULT
         return new OkObjectResult(account);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity>> LoadAsync(Shared.EntityID accountID);
   }

}
