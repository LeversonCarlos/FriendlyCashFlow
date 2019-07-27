#region Using
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
#endregion

namespace FriendCash.Service.Configs.Provider
{
   public class OAuth : OAuthAuthorizationServerProvider
   {

      #region ValidateClientAuthentication
      public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
      {
         try
         {

            // LOAD CLIENT
            var clientId = string.Empty; var clientSecret = string.Empty;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
               context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            if (string.IsNullOrEmpty(clientId))
            {
               context.Validated();
               context.SetError("invalid_clientId", "clientId should be sent");
               return Task.FromResult<object>(null);
            }

            // VALIDATE CLIENT
            Auth.Model.bindClient client = null;
            using (var ctx = new Auth.AuthController())
            {
               client = ctx.GetClient(clientId);
            }
            if (client == null)
            {
               context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
               return Task.FromResult<object>(null);
            }

            // VALIDATE SECRET
            if (client.Type == Auth.Model.viewClient.enumType.NativeConfidential)
            {
               if (string.IsNullOrWhiteSpace(clientSecret))
               {
                  context.SetError("invalid_clientId", "Client secret should be sent.");
                  return Task.FromResult<object>(null);
               }
               else
               {
                  var sescretHash = Model.Base.Hash.Execute(clientSecret);
                  if (client.Secret != sescretHash)
                  {
                     context.SetError("invalid_clientId", "Client secret is invalid.");
                     return Task.FromResult<object>(null);
                  }
               }
            }

            // CHECK CLIENT ACTIVE
            if (!client.Active)
            {
               context.SetError("invalid_clientId", "Client is inactive.");
               return Task.FromResult<object>(null);
            }

            // RESULT
            context.OwinContext.Set<string>("client:AllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("client:RefreshTokenLifeTime", client.RefreshTokenLifetime.ToString());
            context.Validated();
            return Task.FromResult<object>(null);

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GrantResourceOwnerCredentials
      public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
      {
         try
         {

            // ALLOWED ORIGIN
            var allowedOrigin = context.OwinContext.Get<string>("client:AllowedOrigin");
            if (allowedOrigin == null) allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            // REFRESH TOKEN LIFETIME
            var refreshTokenLifeTime = context.OwinContext.Get<string>("client:RefreshTokenLifeTime");
            if (refreshTokenLifeTime == null) { refreshTokenLifeTime = "1"; }

            // LOCATE
            if (String.IsNullOrWhiteSpace(context.UserName) || String.IsNullOrWhiteSpace(context.Password)) 
            { context.Rejected(); return; }
            var userManager = context.OwinContext.GetUserManager<Auth.AppUserManager>();
            var user = await userManager.FindAsync(context.UserName, context.Password);

            // VALIDATE
            if (user == null)
            { context.SetError("invalid_grant", "The user name or password is incorrect."); return; }

            // IDENTITY
            var authIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            var authProps = new AuthenticationProperties(new Dictionary<string, string>
               {
                  { "client:id", (context.ClientId == null) ? string.Empty : context.ClientId },
                  { "client_expires_in", (long.Parse(refreshTokenLifeTime)*60).ToString() },
                  { "SignatureExpiration", "" },
                  { "Roles", FriendCash.Model.Base.Json.Serialize(authIdentity.Claims.Where(x=> x.Type == System.Security.Claims.ClaimTypes.Role).Select(x=> x.Value).ToList() )},
                  { "userName", context.UserName }
               });
            var authTicket = new AuthenticationTicket(authIdentity, authProps);
            context.Validated(authTicket);

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GrantRefreshToken
      public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
      {
         try
         {

            // VALIDATE
            var originalClient = context.Ticket.Properties.Dictionary["client:id"];
            var currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
               context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
               return Task.FromResult<object>(null);
            }

            // APPLY NEW TICKET
            var newIdentity = new System.Security.Claims.ClaimsIdentity(context.Ticket.Identity);

            // REVIEW CLAIMS
            var identifierClaim = newIdentity.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (identifierClaim != null)
            {
               var userManager = context.OwinContext.GetUserManager<Auth.AppUserManager>();
               var user = userManager.FindByIdAsync(identifierClaim.Value).Result;
               if (user != null)
               {
                  user.ReviewClaims(newIdentity).Wait();

                  var expirationClaim = newIdentity.Claims.FirstOrDefault(x => x.Type == "SignatureExpiration");
                  context.Ticket.Properties.Dictionary["SignatureExpiration"] = expirationClaim.Value;

                  var roleClaims = newIdentity.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Role).Select(x => x.Value).ToList();
                  context.Ticket.Properties.Dictionary["Roles"] = FriendCash.Model.Base.Json.Serialize(roleClaims);
               }
            }

            // RESULT
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
            return Task.FromResult<object>(null);

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region TokenEndpoint
      public override Task TokenEndpoint(OAuthTokenEndpointContext context)
      {
         foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
         {
            context.AdditionalResponseParameters.Add(property.Key, property.Value);
         }
         return Task.FromResult<object>(null);
      }
      #endregion

   }
}