using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<IActionResult> DeleteAsync(Shared.EntityID accountID)
      {

         // VALIDATE PARAMETERS
         if (accountID == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_DELETE_PARAMETER });

         // LOCATE ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAccountAsync(accountID));
         if (account == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.ACCOUNT_NOT_FOUND });

         // SAVE CHANGES
         await _AccountRepository.DeleteAccountAsync(accountID);

         // RESULT
         return new OkResult();
      }

   }

   partial interface IAccountService
   {
      Task<IActionResult> DeleteAsync(Shared.EntityID accountID);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_DELETE_PARAMETER = "WARNING_ACCOUNTS_INVALID_DELETE_PARAMETER";
   }

}
