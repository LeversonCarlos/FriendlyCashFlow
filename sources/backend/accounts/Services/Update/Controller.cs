using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpPost("update")]
      [AllowAnonymous]
      public Task<IActionResult> UpdateAsync([FromBody] UpdateVM updateVM) =>
         _AccountService.UpdateAsync(updateVM);

   }

}
