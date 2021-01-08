using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<IActionResult> UpdateAsync(UpdateVM updateVM)
      {

         // VALIDATE PARAMETERS
         if (updateVM == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER });

         // VALIDATE TYPE
         var validateType = await ValidateTypeAsync(updateVM.Type, updateVM.ClosingDay, updateVM.DueDay);
         if (validateType.Length > 0)
            return new BadRequestObjectResult(validateType);

         // VALIDATE DUPLICITY
         var accountsList = await _AccountRepository.SearchAccountsAsync(updateVM.Text);
         if (accountsList.Any(x=> x.AccountID != updateVM.AccountID))
            return new BadRequestObjectResult(new string[] { WARNINGS.ACCOUNT_TEXT_ALREADY_USED });

         // LOCATE ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAccountAsync(updateVM.AccountID));
         if (account == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.ACCOUNT_NOT_FOUND });

         // APPLY CHANGES
         account.Text = updateVM.Text;
         account.Type = updateVM.Type;
         account.ClosingDay = updateVM.ClosingDay;
         account.DueDay = updateVM.DueDay;

         // SAVE CHANGES
         await _AccountRepository.UpdateAccountAsync(account);

         // RESULT
         return new OkResult();
      }

   }

   partial interface IAccountService
   {
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_UPDATE_PARAMETER = "WARNING_ACCOUNTS_INVALID_UPDATE_PARAMETER";
      internal const string ACCOUNT_NOT_FOUND = "WARNING_ACCOUNTS_ACCOUNT_NOT_FOUND";
   }

}
