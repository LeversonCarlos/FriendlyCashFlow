using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Accounts
{

   [Authorize]
   [Route("api/accounts")]
   public partial class AccountController : Base.BaseController
   {
      public AccountController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class AccountsService : Base.BaseService
   {
      public AccountsService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
