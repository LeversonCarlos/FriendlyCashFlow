using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityController
   {

      [HttpPost("auth/token")]
      [AllowAnonymous]
      public Task<ActionResult<TokenVM>> TokenAuthAsync([FromBody] TokenAuthVM param) =>
         _IdentityService.TokenAuthAsync(param);

   }

}
