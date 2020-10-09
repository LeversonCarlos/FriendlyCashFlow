using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      internal async Task<IRefreshToken> CreateRefreshTokenAsync(IUser user)
      {

         // VALIDATE
         if (user == null)
            throw new ArgumentNullException("user", "The User parameter is required for the CreateTokenAsync function on the IdentityService class");
         if (_Settings.Token.RefreshExpirationInSeconds <= 0)
            throw new NullReferenceException("The RefreshExpirationInSeconds property on token settings is required for the CreateRefreshTokenAsync function on the IdentityService class");

         // CREATE INSTANCE
         var refreshToken = RefreshToken.Create(user.UserID, _Settings.Token.RefreshExpirationInSeconds);
         await _RefreshTokenCollection.InsertOneAsync(refreshToken);

         // RESULT
         return refreshToken;

      }

   }

}
