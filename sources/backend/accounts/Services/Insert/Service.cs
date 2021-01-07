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
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_INSERT_PARAMETER });

         // VALIDATE TYPE
         var validateType = await ValidateTypeAsync(insertVM.Type, insertVM.ClosingDay, insertVM.DueDay);
         if (validateType.Length > 0)
            return new BadRequestObjectResult(validateType);

         // VALIDATE DUPLICITY
         var accountsList = await _AccountRepository.SearchAccountsAsync(insertVM.Text);
         if (accountsList != null && accountsList.Length > 0)
            return new BadRequestObjectResult(new string[] { WARNINGS.ACCOUNT_TEXT_ALREADY_USED });

         // ADD NEW ACCOUNT
         var account = new AccountEntity(insertVM.Text, insertVM.Type, insertVM.ClosingDay, insertVM.DueDay);
         await _AccountRepository.InsertAccountAsync(account);

         // RESULT
         return new OkResult();
      }

   }

   partial interface IAccountService
   {
      Task<IActionResult> InsertAsync(InsertVM insertVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_INSERT_PARAMETER = "WARNING_ACCOUNTS_INVALID_INSERT_PARAMETER";
      internal const string ACCOUNT_TEXT_ALREADY_USED = "WARNING_ACCOUNTS_ACCOUNT_TEXT_ALREADY_USED";
   }

}
