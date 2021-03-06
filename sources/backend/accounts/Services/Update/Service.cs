using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{
   partial class AccountService
   {

      public async Task<IActionResult> UpdateAsync(UpdateVM updateVM)
      {

         // VALIDATE PARAMETERS
         if (updateVM == null)
            return Warning(WARNINGS.INVALID_UPDATE_PARAMETER);

         // VALIDATE TYPE
         var validateType = await ValidateTypeAsync(updateVM.Type, updateVM.ClosingDay, updateVM.DueDay);
         if (validateType.Length > 0)
            return Warning(validateType);

         // VALIDATE DUPLICITY
         var accountsList = await _AccountRepository.SearchAsync(updateVM.Text);
         if (accountsList.Any(x => x.AccountID != updateVM.AccountID))
            return Warning(WARNINGS.ACCOUNT_TEXT_ALREADY_USED);

         // LOCATE ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAsync(updateVM.AccountID));
         if (account == null)
            return Warning(WARNINGS.ACCOUNT_NOT_FOUND);

         // APPLY CHANGES
         account.Text = updateVM.Text;
         account.Type = updateVM.Type;
         account.ClosingDay = updateVM.ClosingDay;
         account.DueDay = updateVM.DueDay;

         // SAVE CHANGES
         await _AccountRepository.UpdateAsync(account);

         // RESULT
         return Ok();
      }

   }
}
