using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   partial class CategoryController
   {

      [HttpGet("translations")]
      [AllowAnonymous]
      public Task<ActionResult<Shared.Translations>> TranslationsAsync() =>
         Shared.Translations.CreateAsync(this.HttpContext);

   }

}
