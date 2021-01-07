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
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_CHANGESTATE_PARAMETER });

         // LOCATE ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.GetAccountByIDAsync(changeStateVM.AccountID));
         if (account == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.ACCOUNT_NOT_FOUND });

         // APPLY CHANGES
         account.Active = changeStateVM.State;

         // SAVE CHANGES
         await _AccountRepository.UpdateAccountAsync(account);

         // RESULT
         return new OkResult();
      }

   }

   partial interface IAccountService
   {
      Task<IActionResult> ChangeStateAsync(ChangeStateVM changeStateVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_CHANGESTATE_PARAMETER = "WARNING_ACCOUNTS_INVALID_CHANGESTATE_PARAMETER";
   }

}
