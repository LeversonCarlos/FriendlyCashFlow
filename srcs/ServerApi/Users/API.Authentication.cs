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
            { user = await this.AuthenticateAsync_GetUser(value.RefreshToken); }
            if (user == null) { return this.InformationResponse("USERS_USER_NOT_FOUND_WARNING"); }

            // INITIALIZE RESULT
            var result = new Users.TokenVM
            {
               UserID = user.UserID
            };

            // LOCATE RESOURCE
            var resourceID = await this.AuthenticateAsync_GetResource(user.UserID);

            // IDENTITY
            var claimsIdentity = new ClaimsIdentity(
               new GenericIdentity(user.UserID, "Login"),
               new Claim[] {
                  new Claim(ClaimTypes.NameIdentifier, user.UserID),
                  new Claim(ClaimTypes.Name, user.UserName),
                  new Claim(ClaimTypes.GivenName, user.Text),
                  new Claim(ClaimTypes.System, resourceID)
               }
            );

            // LOCATE ROLES
            var roleList = await this.AuthenticateAsync_GetRoles(user.UserID, resourceID);
            if (roleList == null || roleList.Length == 0)
            {
               await this.SendActivationMailAsync(user.UserID);
               return this.InformationResponse("USERS_USER_NOT_ACTIVATED_WARNING", "USERS_ACTIVATION_INSTRUCTIONS_WAS_SENT_MESSAGE");
            }
            foreach (var role in roleList)
            { claimsIdentity.Claims.Append(new Claim(ClaimTypes.Role, role)); }

            // CREATE TOKEN
            var tokenConfig = this.GetService<Helpers.Token>();
            var securityToken = new SecurityTokenDescriptor
            {
               Issuer = tokenConfig.Configs.Issuer,
               Audience = tokenConfig.Configs.Audience,
               SigningCredentials = tokenConfig.SigningCredentials,
               Subject = claimsIdentity,
               NotBefore = DateTime.UtcNow,
               Expires = DateTime.UtcNow.AddSeconds(tokenConfig.Configs.AccessExpirationInSeconds)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityToken);
            result.AccessToken = tokenHandler.WriteToken(token);

            // REFRESH TOKEN
            result.RefreshToken = result.UserID; /* TODO */

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

      private async Task<UserData> AuthenticateAsync_GetUser(string refreshToken)
      {
         try
         {

            // LOCATE USER
            var user = await this.GetDataQuery()
               .Where(x => x.UserID == refreshToken)
               .FirstOrDefaultAsync();
            return user;

         }
         catch (Exception) { throw; }
      }

      private async Task<string> AuthenticateAsync_GetResource(string userID)
      {
         try
         {
            // TODO
            // TRY TO LOCATE ON PREVIOUS TOKEN LOGINS
            // TAKE THE DEFAULT VALUE FROM THE USER
            return userID;
         }
         catch (Exception) { throw; }
      }

      private async Task<string[]> AuthenticateAsync_GetRoles(string userID, string resourceID)
      {
         try
         {

            // LOAD USER ROLES
            var roleList = await this.dbContext.UserRoles
               .Where(x => x.RowStatus == 1 && x.UserID == userID)
               .Select(x => x.RoleID)
               .ToListAsync();

            // CHECK SIGNATURES
            // TODO

            // RESULT
            return roleList.ToArray();
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
         var service = this.GetService<UsersService>();
         return await service.AuthenticateAsync(value);
      }

   }

}
