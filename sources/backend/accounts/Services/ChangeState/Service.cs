using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Accounts
{
   partial class AccountService
   {

      public async Task<IActionResult> ChangeStateAsync(ChangeStateVM changeStateVM)
      {

         // VALIDATE PARAMETERS
         if (changeStateVM == null)
            return Warning(WARNINGS.INVALID_CHANGESTATE_PARAMETER);

         // LOCATE ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAccountAsync(changeStateVM.AccountID));
         if (account == null)
            return Warning(WARNINGS.ACCOUNT_NOT_FOUND);

         // APPLY CHANGES
         account.Active = changeStateVM.State;

         // SAVE CHANGES
         await _AccountRepository.UpdateAccountAsync(account);

         // RESULT
         return Ok();
      }

   }
}
