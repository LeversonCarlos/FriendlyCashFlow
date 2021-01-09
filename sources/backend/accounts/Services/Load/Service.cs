using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<ActionResult<IAccountEntity>> LoadAsync(string id)
      {

         // VALIDATE PARAMETERS
         if (!Shared.EntityID.TryParse(id, out var accountID))
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_LOAD_PARAMETER });

         // LOAD ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAccountAsync(accountID));

         // RESULT
         return new OkObjectResult(account);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity>> LoadAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_LOAD_PARAMETER = "WARNING_ACCOUNTS_INVALID_LOAD_PARAMETER";
   }

}
