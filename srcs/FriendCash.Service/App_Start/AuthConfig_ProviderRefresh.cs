#region Using
// using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;
#endregion

namespace FriendCash.Service.Configs.Provider
{
   public class OAuthRefresh : IAuthenticationTokenProvider
   {

      #region Create
      public void Create(AuthenticationTokenCreateContext context)
      {
         throw new NotImplementedException();
      }
      #endregion

      #region CreateAsync
      public async Task CreateAsync(AuthenticationTokenCreateContext context)
      {

         var clientId = context.Ticket.Properties.Dictionary["client:id"];
         if (string.IsNullOrEmpty(clientId))
         { return; }

         var refreshTokenId = Guid.NewGuid().ToString("n");
         

         using (var authController = new Auth.AuthController())
         {
            var refreshTokenLifeTime = context.OwinContext.Get<string>("client:RefreshTokenLifeTime");

            var token = new Auth.Model.bindToken()
            {
               Id = Model.Base.Hash.Execute(refreshTokenId),
               ClientId = clientId,
               UserName = context.Ticket.Identity.Name,
               IssuedTime = DateTime.UtcNow,
               ExpiryTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedTime;
            context.Ticket.Properties.ExpiresUtc = token.ExpiryTime;
            token.Ticket = context.SerializeTicket();

            var result = await authController.AddToken(token);
            if (result) { context.SetToken(refreshTokenId); }

         }
      }
      #endregion

      #region Receive
      public void Receive(AuthenticationTokenReceiveContext context)
      {
         throw new NotImplementedException();
      }
      #endregion

      #region ReceiveAsync
      public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
      {

         var allowedOrigin = context.OwinContext.Get<string>("client:AllowedOrigin");
         context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

         string tokenId = Model.Base.Hash.Execute(context.Token);

         using (var authController = new Auth.AuthController())
         {
            var refreshToken = await authController.GetTokenByID(tokenId);
            if (refreshToken != null)
            {
               context.DeserializeTicket(refreshToken.Ticket);
               var result = await authController.RemoveToken(tokenId);
            }
         }
      }
      #endregion

   }
}