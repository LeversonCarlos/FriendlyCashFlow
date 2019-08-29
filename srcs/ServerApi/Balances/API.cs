using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Balances
{

   /*
   [Authorize]
   [Route("api/patterns")]
   public partial class AccountController : Base.BaseController
   {
      public AccountController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }
   */

   internal partial class BalancesService : Base.BaseService
   {
      public BalancesService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
