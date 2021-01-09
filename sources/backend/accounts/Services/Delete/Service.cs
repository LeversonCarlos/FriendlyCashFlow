using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<IActionResult> DeleteAsync(string id)
      {

         // VALIDATE PARAMETERS
         if (!Shared.EntityID.TryParse(id, out var accountID))
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
      Task<IActionResult> DeleteAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_DELETE_PARAMETER = "WARNING_ACCOUNTS_INVALID_DELETE_PARAMETER";
   }

}
