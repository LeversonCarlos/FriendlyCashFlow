#region Using
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Configs.Provider
{
   public class TokenFormat : ISecureDataFormat<AuthenticationTicket>
   {
      private readonly string _issuer = string.Empty;
      private readonly string _audienceID= string.Empty;
      private byte[] _audienceSecret = null;
      private SigningCredentials _signingCredentials { get; set; }

      #region New
      public TokenFormat(string issuer, string audienceID, byte[] audienceSecret)
      { 
         this._issuer = issuer;
         this._audienceID = audienceID;
         this._audienceSecret = audienceSecret;
         this._signingCredentials = new Thinktecture.IdentityModel.Tokens.HmacSigningCredentials(this._audienceSecret);
      }
      #endregion

      #region Protect
      public string Protect(AuthenticationTicket data)
      {
         try
         {

            // VALIDATE
            if (data == null) { throw new ArgumentNullException("data"); }

            // PARAMETERS
            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            // TOKEN
            var token = new JwtSecurityToken(this._issuer, this._audienceID, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, this._signingCredentials);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region Unprotect
      public AuthenticationTicket Unprotect(string protectedText)
      {
         try
         {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenParameters = new TokenValidationParameters()
            {
               ValidIssuer = this._issuer,
               ValidAudience = this._audienceID,
               IssuerSigningKey = this._signingCredentials.SigningKey,
               RequireExpirationTime = true
            };

            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(protectedText, tokenParameters, out validatedToken);
            var claimsIdentity = new System.Security.Claims.ClaimsIdentity(claimsPrincipal.Identity, claimsPrincipal.Claims);

            return new AuthenticationTicket(claimsIdentity, null);
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}