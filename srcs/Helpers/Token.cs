using Microsoft.IdentityModel.Tokens;

namespace FriendlyCashFlow.Helpers
{
   internal class Token
   {

      public readonly AppSettingsToken Configs;
      public readonly SymmetricSecurityKey SecurityKey;
      public readonly SigningCredentials SigningCredentials;

      public Token(AppSettings appSettings)
      {

         this.Configs = appSettings.Token;

         var secretBytes = System.Text.Encoding.ASCII.GetBytes(this.Configs.Secret);
         this.SecurityKey = new SymmetricSecurityKey(secretBytes);

         this.SigningCredentials = new SigningCredentials(this.SecurityKey, SecurityAlgorithms.HmacSha256Signature);
      }

   }
}
