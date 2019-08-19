using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow
{
   partial class Startup
   {

      private void AddAuthServices(IServiceCollection services)
      {

         // CREATE INSTANCE FOR THE TOKEN CONFIG
         var appSettings = this.GetAppSettings(services);
         var tokenConfig = new Helpers.Token(appSettings);
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
                        //var userID = long.Parse(context.Principal.Identity.Name);
                        return Task.CompletedTask;
                     }
                     catch (Exception) { throw; }
                  }
               };
            });

         // CONFIGURE INJECTION FOR USER SERVICES
         // services.AddScoped<Shared.Users.IUserService, Shared.Users.UserService>();

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
