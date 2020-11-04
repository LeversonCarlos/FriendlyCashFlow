using System;
using System.Threading.Tasks;

namespace Elesse.Identity
{

   partial class IdentityService
   {

      internal async Task<IRefreshToken> CreateRefreshTokenAsync(IUserEntity user)
      {

         // VALIDATE
         if (user == null)
            throw new ArgumentNullException("user", "The User parameter is required for the CreateTokenAsync function on the IdentityService class");
         if (_Settings.Token.RefreshExpirationInSeconds <= 0)
            throw new ArgumentException("The RefreshExpirationInSeconds property on token settings is required for the CreateRefreshTokenAsync function on the IdentityService class");

         // CREATE INSTANCE
         var refreshTokenExpiration = DateTime.UtcNow.AddSeconds(_Settings.Token.RefreshExpirationInSeconds);
         var refreshToken = RefreshToken.Create(user.UserID, refreshTokenExpiration);
         await _TokenRepository.AddRefreshTokenAsync(refreshToken);

         // RESULT
         return refreshToken;

      }

   }

}
