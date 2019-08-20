using System;
using System.Collections.Generic;
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
            {
               user = await this.AuthenticateAsync_GetUser(value);
               if (user == null) { return this.InformationResponse("USERS_USER_NOT_FOUND_WARNING"); }
            }
            else if (value.GrantType == "refresh_token")
            {
               user = await this.AuthenticateAsync_GetUser(value.RefreshToken);
               if (user == null) { return this.InformationResponse("USERS_TOKEN_HAS_EXPIRED_WARNING"); }
            }
            var result = new Users.TokenVM { UserID = user.UserID };

            // CHECK ACTIVATION
            if (user.RowStatus == (short)Base.enRowStatus.Temporary)
            {
               await this.SendActivationMailAsync(user.UserID);
               return this.InformationResponse("USERS_USER_NOT_ACTIVATED_WARNING", "USERS_ACTIVATION_INSTRUCTIONS_WAS_SENT_MESSAGE");
            }

            // GENERATE IDENTITY
            var roleList = await this.AuthenticateAsync_GetRoles(user.UserID, user.SelectedResourceID);
            var claimsList = new List<Claim>{
               new Claim(ClaimTypes.NameIdentifier, user.UserID),
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.GivenName, user.Text),
               new Claim(ClaimTypes.System, user.SelectedResourceID)
            };
            foreach (var role in roleList)
            { claimsList.Add(new Claim(ClaimTypes.Role, role)); }
            var claimsIdentity = new ClaimsIdentity(
               new GenericIdentity(user.UserID, "Login"),
               claimsList.ToArray()
            );

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
            result.RefreshToken = await this.AuthenticateAsync_GetRefreshToken(user.UserID, tokenConfig.Configs.RefreshExpirationInSeconds);

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
            var user = await this.dbContext.Users
               .Where(x =>
                  x.RowStatus != (short)Base.enRowStatus.Removed &&
                  x.UserName == value.UserName &&
                  x.PasswordHash == passwordHash)
               .FirstOrDefaultAsync();
            return user;

         }
         catch (Exception) { throw; }
      }

      private async Task<UserData> AuthenticateAsync_GetUser(string refreshToken)
      {
         try
         {

            // LOCATE TOKEN
            var token = await this.dbContext.UserTokens
               .Where(x => x.RefreshToken == refreshToken)
               .FirstOrDefaultAsync();
            if (token == null) { return null; }

            // REMOVE TOKEN
            this.dbContext.UserTokens.Remove(token);
            await this.dbContext.SaveChangesAsync();

            // CHECK EXPIRATION
            if (token.ExpirationDate < DateTime.UtcNow) { return null; }

            // LOCATE USER
            var user = await this.GetDataQuery()
               .Where(x => x.UserID == token.UserID)
               .FirstOrDefaultAsync();
            return user;

         }
         catch (Exception) { throw; }
      }

      private async Task<string[]> AuthenticateAsync_GetRoles(string userID, string resourceID)
      {
         try
         {

            // LOAD USER ROLES
            var roleList = await this.dbContext.UserRoles
               .Where(x => x.RowStatus == 1 && x.UserID == userID && x.ResourceID == resourceID)
               .Select(x => x.RoleID)
               .ToListAsync();

            // CHECK SIGNATURES
            // TODO

            // RESULT
            return roleList.ToArray();
         }
         catch (Exception) { throw; }
      }

      private async Task<string> AuthenticateAsync_GetRefreshToken(string userID, int expirationInSeconds)
      {
         try
         {

            // INITIALIZE
            var token = new UserTokenData
            {
               UserID = userID,
               ExpirationDate = DateTime.UtcNow.AddSeconds(expirationInSeconds)
            };

            // RANDOMIZE TOKEN
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
               var randomNumber = new byte[32];
               rng.GetBytes(randomNumber);
               token.RefreshToken = Convert.ToBase64String(randomNumber);
               token.RefreshToken = token.RefreshToken
                  .Replace("+", string.Empty)
                  .Replace("&", string.Empty)
                  .Replace("=", string.Empty)
                  .Replace("/", string.Empty);
            }

            // SAVE IT
            await this.dbContext.UserTokens.AddAsync(token);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return token.RefreshToken;
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
