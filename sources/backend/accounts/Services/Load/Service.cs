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
         var account = await _AccountRepository.LoadAsync(accountID);

         // RESULT
         return Ok(account);
      }

   }
}
