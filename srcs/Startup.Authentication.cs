using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddAuthentication(IServiceCollection services)
      {

         // CREATE INSTANCE FOR THE TOKEN CONFIG
         var tokenConfig = new Helpers.Token(this.AppSettings);
         services.AddSingleton(tokenConfig);

         // CONFIGURE JWT AUTHENTICATION
         services.AddAuthentication(x =>
            {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                  ValidIssuer = tokenConfig.Configs.Issuer,
                  ValidateIssuer = true,
                  ValidAudience = tokenConfig.Configs.Audience,
                  ValidateAudience = true,
                  IssuerSigningKey = tokenConfig.SecurityKey,
                  ValidateIssuerSigningKey = true,
                  ValidateLifetime = true,
                  ClockSkew = System.TimeSpan.Zero
               };
               x.Events = new JwtBearerEvents
               {
                  OnTokenValidated = (TokenValidatedContext context) =>
                  {
                     try
                     {
                        var user = context.HttpContext.RequestServices.GetRequiredService<Helpers.User>();
                        user.Identity = context.Principal.Identity;
                        user.UserID = user.Identity.Name;
                        user.ResourceID = ((System.Security.Claims.ClaimsPrincipal)context.Principal).Claims
                           .Where(claim => claim.Type == System.Security.Claims.ClaimTypes.System)
                           .Select(claim => claim.Value)
                           .FirstOrDefault();
                        return Task.CompletedTask;
                     }
                     catch (Exception) { throw; }
                  }
               };
            });

         services.AddAuthorization(auth =>
            {
               auth
                  .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                  .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                  .RequireAuthenticatedUser()
                  .Build());
            });

      }

   }
}
