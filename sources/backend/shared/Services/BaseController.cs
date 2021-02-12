using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{

   public abstract class BaseController : Controller
   {

      [HttpGet("translations")]
      [AllowAnonymous]
      public Task<ActionResult<Translations>> TranslationsAsync() =>
         Translations.CreateAsync(this.HttpContext);

   }

}
