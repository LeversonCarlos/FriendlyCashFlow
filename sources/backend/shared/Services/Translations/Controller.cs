using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{

   partial class SharedController
   {

      [HttpGet("translations")]
      [AllowAnonymous]
      public Task<ActionResult<Shared.Translations>> TranslationsAsync() =>
         Shared.Translations.CreateAsync(this.HttpContext);

   }

}
