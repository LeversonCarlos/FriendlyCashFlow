using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Balances
{

   [Route("api/balances")]
   [Authorize]
   public partial class BalanceController : Shared.BaseController
   {

      internal readonly IBalanceService _BalanceService;

      public BalanceController(IBalanceService balanceService)
      {
         _BalanceService = balanceService;
      }

   }

}
