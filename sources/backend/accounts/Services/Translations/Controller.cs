using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   partial class AccountController
   {

      [HttpGet("translations")]
      [AllowAnonymous]
      public Shared.Translations TranslationsAsync() =>
         Shared.Translations.Create(this.HttpContext);

   }

}
