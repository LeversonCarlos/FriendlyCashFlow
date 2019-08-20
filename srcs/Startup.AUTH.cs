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
                        var user = context.HttpContext.RequestServices.GetRequiredService<Helpers.User>();
                        user.Identity = context.Principal.Identity;
                        user.UserID = user.Identity.Name;
                        return Task.CompletedTask;
                     }
                     catch (Exception) { throw; }
                  }
               };
            });

         // CONFIGURE INJECTION FOR USER HELPER
         services.AddScoped<Helpers.User>();
         services.AddScoped<API.Users.UsersService>();

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
