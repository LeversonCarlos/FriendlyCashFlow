using FriendlyCashFlow.Shared;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   internal class CreateTokenInteractor : Interactor<IUser, IdentitySettings, IUser, TokenVM>
   {

      public CreateTokenInteractor(IMongoDatabase mongoDatabase, IdentitySettings settings) :
         base(mongoDatabase, settings, IdentityService.CollectionName)
      { }

      public override async Task<TokenVM> ExecuteAsync(IUser user)
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
            var userRoles = await GetRoles(user);
            foreach (var userRole in userRoles)
               claimsList.Add(new Claim(ClaimTypes.Role, userRole));

            // GENERATE IDENTITY
            var identity = new ClaimsIdentity(
               new GenericIdentity(user.UserID, "Login"),
               claimsList.ToArray()
            );

            // CREATE ACCESS TOKEN
            var accessToken = Helpers.Token.CreateToken(identity, Settings.Token);

            // CREATE REFRESH TOKEN
            // TODO

            // RESULT
            var token = new TokenVM
            {
               UserID = user.UserID,
               AccessToken = accessToken
            };
            return token;

         }
         catch (Exception) { throw; }
      }

      Task<string[]> GetRoles(IUser user)
      {
         return Task.FromResult(new string[] { "editor" });
      }

   }
}
