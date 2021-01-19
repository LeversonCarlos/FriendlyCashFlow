using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<ActionResult<IAccountEntity[]>> ListAsync()
      {

         // LOAD ACCOUNTS
         var accountsList = await _AccountRepository.ListAccountsAsync();

         // RESULT
         return Shared.Results.Ok(accountsList);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity[]>> ListAsync();
   }

}
