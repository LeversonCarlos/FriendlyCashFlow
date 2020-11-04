using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityController
   {

      [HttpPost("auth/user")]
      [AllowAnonymous]
      public Task<ActionResult<TokenVM>> UserAuthAsync([FromBody] UserAuthVM param) =>
         _IdentityService.UserAuthAsync(param);

   }

}
