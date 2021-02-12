using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPut("change-state")]
      public Task<IActionResult> ChangeStateAsync([FromBody] ChangeStateVM changeStateVM) =>
         _AccountService.ChangeStateAsync(changeStateVM);

   }

}
