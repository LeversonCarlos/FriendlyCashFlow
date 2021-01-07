using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   [Route("api/accounts")]
   [Authorize]
   public partial class AccountController : Controller
   {

      internal readonly IAccountService _AccountService;

      public AccountController(IAccountService accountService)
      {
         _AccountService = accountService;
      }

   }

}
