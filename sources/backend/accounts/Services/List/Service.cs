using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<ActionResult<IAccountEntity[]>> ListAsync()
      {

         // LOAD ACCOUNTS
         var accountsList = await _AccountRepository.ListAccountsAsync();

         // RESULT
         return Ok(accountsList);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity[]>> ListAsync();
   }

}
