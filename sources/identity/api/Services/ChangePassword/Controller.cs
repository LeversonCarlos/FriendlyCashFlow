using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityController
   {

      [Authorize]
      [HttpPut("change-password")]
      public Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordVM param) =>
         _IdentityService.ChangePasswordAsync(this.User.Identity, param);

   }

}
