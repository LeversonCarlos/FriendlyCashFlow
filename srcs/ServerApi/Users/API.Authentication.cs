using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

            // INITIALIZE RESULT
            var result = new Users.TokenVM
            {
               UserID = user.UserID
            };

            // IDENTITY
            var claimsIdentity = new ClaimsIdentity(
               new GenericIdentity(user.UserID, "Login"),
               new Claim[] {
                  new Claim(ClaimTypes.NameIdentifier, user.UserID),
                  new Claim(ClaimTypes.Name, user.UserName),
                  new Claim(ClaimTypes.GivenName, user.Text),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserID)
               }
            );

            // CREATE TOKEN
            var tokenConfig = this.GetService<Helpers.Token>();
            var securityToken = new SecurityTokenDescriptor
            {
               Issuer = tokenConfig.Configs.Issuer,
               Audience = tokenConfig.Configs.Audience,
               SigningCredentials = tokenConfig.GetSigningCredentials(),
               Subject = claimsIdentity,
               NotBefore = DateTime.UtcNow,
               Expires = DateTime.UtcNow.AddSeconds(tokenConfig.Configs.AccessExpirationInSeconds)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityToken);
            result.AccessToken = tokenHandler.WriteToken(token);

            return this.OkResponse(result);
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
