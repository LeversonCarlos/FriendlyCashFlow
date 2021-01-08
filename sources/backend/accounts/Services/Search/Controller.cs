using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("search/{searchText}")]
      public Task<ActionResult<IAccountEntity[]>> SearchAsync(string searchText) =>
         _AccountService.SearchAsync(searchText);

   }

}
