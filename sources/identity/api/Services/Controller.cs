using Microsoft.AspNetCore.Mvc;

namespace Elesse.Identity
{

   [Route("api/identity")]
   public partial class IdentityController : Controller
   {

      internal readonly IIdentityService _IdentityService;

      public IdentityController(IIdentityService identityService)
      {
         _IdentityService = identityService;
      }

   }

}
