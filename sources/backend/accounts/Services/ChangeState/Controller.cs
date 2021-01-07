using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPut("change-state")]
      public Task<IActionResult> ChangeStateAsync([FromBody] ChangeStateVM changeStateVM) =>
         _AccountService.ChangeStateAsync(changeStateVM);

   }

}
