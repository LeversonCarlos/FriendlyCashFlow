using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityController
   {

      [HttpPost("auth/user")]
      [AllowAnonymous]
      public Task<IActionResult> UserAuthAsync([FromBody] UserAuthVM param) =>
         _IdentityService.UserAuthAsync(param);

   }

}
