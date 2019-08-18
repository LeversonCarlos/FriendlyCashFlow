using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Users
{

   partial class UserController
   {

      [AllowAnonymous]
      [HttpPost("auth")]
      public async Task<ActionResult<Users.AuthVM>> AuthenticateAsync([FromBody]Users.AuthVM value)
      {
         //var userService = this.GetInjectedService<Users.IUserService>();
         using (var service = new UsersService(this.serviceProvider))
         { return await service.AuthenticateAsync(value); }
      }

   }

   partial class UsersService
   {

      public async Task<ActionResult<Users.AuthVM>> AuthenticateAsync(Users.AuthVM value)
      {
         return value;
      }

   }
}
