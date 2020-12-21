using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityService
   {

      internal async Task<TokenVM> CreateAccessTokenAsync(IUserEntity user)
      {

         if (user == null)
            throw new ArgumentNullException("user", "The User parameter is required for the CreateTokenAsync function on the IdentityService class");

         // INITIALIZE CLAIMS
         var claimsList = new List<Claim>{
               new Claim(ClaimTypes.NameIdentifier, user.UserName)
               // new Claim(ClaimTypes.GivenName, user.UserDescription)
            };

         // ADD USER ROLES AS CLAIMS 
         var userRoles = new string[] { "editor" }; // TODO
         foreach (var userRole in userRoles)
            claimsList.Add(new Claim(ClaimTypes.Role, userRole));

         // GENERATE IDENTITY
         var identity = new ClaimsIdentity(
            new GenericIdentity(user.UserID, "Login"),
            claimsList.ToArray()
         );

         // CREATE ACCESS TOKEN
         var tokenDescriptor = Helpers.Token.GetTokenDescriptor(identity, _Settings.Token);
         var accessToken = Helpers.Token.CreateToken(tokenDescriptor);

         // CREATE REFRESH TOKEN
         var refreshToken = await CreateRefreshTokenAsync(user);

         // RESULT
         var token = new TokenVM
         {
            UserID = user.UserID,
            AccessToken = accessToken,
            RefreshToken = refreshToken.TokenID
         };
         return token;

      }

   }

}
