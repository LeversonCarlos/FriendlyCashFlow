using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts
{

   [Route("api/accounts")]
   public partial class AccountController : Controller
   {

      internal readonly IAccountService _AccountService;

      public AccountController(IAccountService accountService)
      {
         _AccountService = accountService;
      }

   }

}
