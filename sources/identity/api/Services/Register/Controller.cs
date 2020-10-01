using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityController
   {

      [HttpPost("")]
      [AllowAnonymous]
      public Task<IActionResult> RegisterAsync(RegisterVM registerVM) =>
         _IdentityService.RegisterAsync(registerVM);

   }

}
