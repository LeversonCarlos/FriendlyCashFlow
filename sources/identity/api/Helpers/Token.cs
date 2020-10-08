using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FriendlyCashFlow.Identity.Helpers
{

   internal class Token
   {

      internal static SymmetricSecurityKey GetSecurityKey(TokenSettings settings)
      {

         if (settings == null || string.IsNullOrEmpty(settings.SecuritySecret))
            throw new ArgumentException("The SecuritySecret property of the TokenSettings is required to build a SecurityKey");

         var secretBytes = Encoding.UTF8.GetBytes(settings.SecuritySecret);
         var securityKey = new SymmetricSecurityKey(secretBytes);
         return securityKey;
      }

      internal static SigningCredentials GetSigningCredentials(TokenSettings settings)
      {
         var securityKey = GetSecurityKey(settings);
         var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
         return signingCredentials;
      }

      internal static string CreateToken(ClaimsIdentity identity, TokenSettings settings)
      {

         // TOKEN DESCRIPTOR
         var tokenDescriptor = new SecurityTokenDescriptor
         {
            Issuer = settings.Issuer,
            Audience = settings.Audience,
            SigningCredentials = GetSigningCredentials(settings),
            Subject = identity,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(settings.AccessExpirationInSeconds)
         };

         // CREATE TOKEN
         var tokenHandler = new JwtSecurityTokenHandler();
         var securityToken = tokenHandler.CreateToken(tokenDescriptor);
         var accessToken = tokenHandler.WriteToken(securityToken);

         // RESULT
         return accessToken;
      }

   }

}
