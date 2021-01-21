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
            return Warning(WARNINGS.INVALID_LOAD_PARAMETER);

         // LOAD ACCOUNT
         var account = await _AccountRepository.LoadAccountAsync(accountID);

         // RESULT
         return Ok(account);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity>> LoadAsync(string id);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_LOAD_PARAMETER = "INVALID_ACCOUNTID_PARAMETER";
   }

}
