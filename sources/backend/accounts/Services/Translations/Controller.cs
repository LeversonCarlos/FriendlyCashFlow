using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("translations")]
      [AllowAnonymous]
      public Task<Shared.Translations> TranslationsAsync() =>
         Shared.Translations.CreateAsync(this.HttpContext);

   }

}
