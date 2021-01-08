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
         var accountsList = (AccountEntity[])(await _AccountRepository.ListAccountsAsync());

         // RESULT
         return new OkObjectResult(accountsList);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity[]>> ListAsync();
   }

}