using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      async Task<TokenVM> CreateTokenAsync(IUser user)
      {
         try
         {

            // INITIALIZE CLAIMS
            var claimsList = new List<Claim>{
               new Claim(ClaimTypes.NameIdentifier, user.UserID),
               new Claim(ClaimTypes.Name, user.UserName),
               // new Claim(ClaimTypes.GivenName, user.Text)
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
            var accessToken = Helpers.Token.CreateToken(identity, _Settings.Token);

            // CREATE REFRESH TOKEN
            // TODO

            // RESULT
            var token = new TokenVM
            {
               UserID = user.UserID,
               AccessToken = accessToken
            };
            await Task.CompletedTask;
            return token;

         }
         catch (Exception) { throw; }
      }

   }

}
