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
         var accountsList = await _AccountRepository.SearchAccountsAsync(searchText);

         // RESULT
         return Shared.Results.Ok(accountsList);
      }

   }

   partial interface IAccountService
   {
      Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText);
   }

}
