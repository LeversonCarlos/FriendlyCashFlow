using Microsoft.AspNetCore.Mvc;

namespace Elesse.Identity
{

   [Route("api/identity")]
   public partial class IdentityController : Shared.BaseController
   {

      internal readonly IIdentityService _IdentityService;

      public IdentityController(IIdentityService identityService)
      {
         _IdentityService = identityService;
      }

   }

}
