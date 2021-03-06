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
            return Warning(WARNINGS.INVALID_DELETE_PARAMETER);

         // LOCATE ACCOUNT
         var account = (AccountEntity)(await _AccountRepository.LoadAccountAsync(accountID));
         if (account == null)
            return Warning(WARNINGS.ACCOUNT_NOT_FOUND);

         // SAVE CHANGES
         await _AccountRepository.DeleteAccountAsync(accountID);

         // TRACK EVENT
         _InsightsService.TrackEvent("Account Service Delete");

         // RESULT
         return Ok();
      }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_DELETE_PARAMETER = "INVALID_ACCOUNTID_PARAMETER";
   }

}
