using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Users
{

   partial class UsersService
   {

      public async Task<ActionResult<Users.TokenVM>> AuthenticateAsync(Users.AuthVM value)
      {
         try
         {

            // LOCATE USER
            UserData user = null;
            if (value.GrantType == "password")
            { user = await this.AuthenticateAsync_GetUser(value); }
            else if (value.GrantType == "refresh_token")
            {/* TODO */ }
            if (user == null) { return this.InformationResponse("USERS_AUTHENTICATE_NOT_FOUND_WARNING"); }

            // CREATE TOKEN
            var token = new Users.TokenVM
            {
               UserID = user.UserID
            };

            return this.OkResponse(token);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<UserData> AuthenticateAsync_GetUser(Users.AuthVM value)
      {
         try
         {

            // HASH THE PASSWORD
            var cryptService = this.GetService<Helpers.Crypt>();
            var passwordHash = cryptService.Encrypt(value.Password);

            // LOCATE USER
            var user = await this.GetDataQuery()
               .Where(x => x.UserName == value.UserName && x.PasswordHash == passwordHash)
               .FirstOrDefaultAsync();
            return user;

         }
         catch (Exception) { throw; }
      }

   }

   partial class UserController
   {

      [AllowAnonymous]
      [HttpPost("auth")]
      public async Task<ActionResult<Users.TokenVM>> AuthenticateAsync([FromBody]Users.AuthVM value)
      {
         //var userService = this.GetInjectedService<Users.IUserService>();
         using (var service = new UsersService(this.serviceProvider))
         { return await service.AuthenticateAsync(value); }
      }

   }

}
