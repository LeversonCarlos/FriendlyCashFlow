using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("list")]
      public Task<ActionResult<IAccountEntity[]>> ListAsync() =>
         _AccountService.ListAsync();

   }

}
