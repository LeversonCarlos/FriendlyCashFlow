using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("list")]
      public Task<ActionResult<IAccountEntity[]>> ListAsync() =>
         _AccountService.ListAsync();

   }

}
