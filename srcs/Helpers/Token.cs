using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FriendlyCashFlow.Helpers
{
   internal class Token
   {

      private readonly IOptions<AppSettings> _appSettings;
      public Token([FromServices] IOptions<AppSettings> appSettings) { this._appSettings = appSettings; }

      public AppSettingsToken Configs { get { return this._appSettings.Value.Token; } }

      public SigningCredentials GetSigningCredentials()
      {
         var secretBytes = System.Text.Encoding.ASCII.GetBytes(this.Configs.Secret);
         var securityKey = new SymmetricSecurityKey(secretBytes);
         var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
         return signingCredentials;
      }

   }
}
