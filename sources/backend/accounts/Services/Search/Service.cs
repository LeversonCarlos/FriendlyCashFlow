using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Accounts
{

   partial class AccountService
   {

      public async Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText)
      {

         // SEARCH ACCOUNTS
         var accountsList = (AccountEntity[])(await _AccountRepository.SearchAccountsAsync(searchText));

         // RESULT
         return new OkObjectResult(accountsList);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText);
   }

}
