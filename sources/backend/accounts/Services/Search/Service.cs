using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{
   partial class AccountService
   {

      public async Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText)
      {

         // SEARCH ACCOUNTS
         var accountsList = await _AccountRepository.SearchAsync(searchText);

         // RESULT
         return Ok(accountsList);
      }

   }
}
