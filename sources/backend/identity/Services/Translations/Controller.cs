using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Identity
{

   partial class IdentityController
   {

      [HttpGet("translations")]
      [AllowAnonymous]
      public Task<ActionResult<Shared.Translations>> TranslationsAsync() =>
         Shared.Translations.CreateAsync(this.HttpContext);

   }

}
