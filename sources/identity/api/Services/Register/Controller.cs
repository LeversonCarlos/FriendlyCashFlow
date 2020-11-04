using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityController
   {

      [HttpPost("user")]
      [AllowAnonymous]
      public Task<IActionResult> RegisterAsync([FromBody] RegisterVM registerVM) =>
         _IdentityService.RegisterAsync(registerVM);

   }

}
