using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{

   [Route("api/shared")]
   [Authorize]
   public partial class SharedController : BaseController
   {

      // internal readonly IAccountService _AccountService;

      public SharedController()
      {
         // _AccountService = accountService;
      }

   }

}
