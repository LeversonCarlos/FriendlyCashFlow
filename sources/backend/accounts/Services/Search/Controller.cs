using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("search/{searchText}")]
      public Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText) =>
         _AccountService.SearchAsync(searchText);

   }

}
