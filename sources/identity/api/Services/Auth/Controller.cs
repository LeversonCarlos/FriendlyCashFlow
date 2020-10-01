using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityController
   {

      [HttpPost("auth/user")]
      [AllowAnonymous]
      public Task<IActionResult> AuthUserAsync([FromBody] AuthUserVM authUserVM) =>
         _IdentityService.AuthUserAsync(authUserVM);

   }

}
