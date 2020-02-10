using Microsoft.AspNetCore.Mvc;
using System;

namespace FriendlyCashFlow.API.Users
{

   internal partial class UsersService : Base.BaseService
   {
      public UsersService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

   // [Authorize]
   [Route("api/users")]
   public partial class UserController : Base.BaseController
   {
      public UserController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

}
