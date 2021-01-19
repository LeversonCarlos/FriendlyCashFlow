using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<IActionResult> InsertAsync(InsertVM insertVM)
      {

         // VALIDATE PARAMETERS
         if (insertVM == null)
            return Warning(WARNINGS.INVALID_INSERT_PARAMETER);

         // VALIDATE TYPE
         var validateType = await ValidateTypeAsync(insertVM.Type, insertVM.ClosingDay, insertVM.DueDay);
         if (validateType.Length > 0)
            return Warning(validateType);

         // VALIDATE DUPLICITY
         var accountsList = await _AccountRepository.SearchAccountsAsync(insertVM.Text);
         if (accountsList != null && accountsList.Length > 0)
            return Warning(WARNINGS.ACCOUNT_TEXT_ALREADY_USED);

         // ADD NEW ACCOUNT
         var account = new AccountEntity(insertVM.Text, insertVM.Type, insertVM.ClosingDay, insertVM.DueDay);
         await _AccountRepository.InsertAccountAsync(account);

         // TRACK EVENT
         _InsightsService.TrackEvent("Account Service Insert");

         // RESULT
         return Shared.Results.Ok();
      }

   }

   partial interface IAccountService
   {
      Task<IActionResult> InsertAsync(InsertVM insertVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_INSERT_PARAMETER = "INVALID_INSERT_PARAMETER";
      internal const string ACCOUNT_TEXT_ALREADY_USED = "ACCOUNT_TEXT_ALREADY_USED";
   }

}
