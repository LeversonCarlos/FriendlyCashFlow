using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Elesse.Identity.Helpers
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddIdentityService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddSingleton(s => configs.GetSection("Identity").Get<IdentitySettings>())
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITokenRepository, TokenRepository>()
            .AddScoped<IIdentityService, IdentityService>();
         services
            .AddAuthentication(SetAuthenticationOptions)
            .AddJwtBearer(options => SetJwtBearerOptions(options, configs));
         services
            .AddAuthorization(SetAuthorizationOptions);
         mvcBuilder
            .AddApplicationPart(Assembly.GetAssembly(typeof(IdentityController)));
         return mvcBuilder;
      }

      static void SetAuthenticationOptions(AuthenticationOptions options)
      {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }

      static void SetJwtBearerOptions(JwtBearerOptions options, IConfiguration configs)
      {
         options.RequireHttpsMetadata = false;
         options.SaveToken = true;

         var settings = configs.GetSection("Identity").Get<IdentitySettings>();
         options.TokenValidationParameters = new TokenValidationParameters
         {
            ValidIssuer = settings.Token.Issuer,
            ValidateIssuer = true,
            ValidAudience = settings.Token.Audience,
            ValidateAudience = true,
            IssuerSigningKey = Helpers.Token.GetSecurityKey(settings.Token),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = System.TimeSpan.Zero
         };

         options.Events = new JwtBearerEvents
         {
            OnTokenValidated = (TokenValidatedContext context) =>
            {
               var user = (User)context.HttpContext.RequestServices.GetRequiredService<IUser>();
               var identity = context.Principal.Identity as ClaimsIdentity;
               user.UserID = identity.Name;
               user.UserName = identity.Claims
                  .Where(claim => claim.Type == ClaimTypes.NameIdentifier)
                  .Select(claim => claim.Value)
                  .FirstOrDefault();
               user.Roles = identity.Claims
                  .Where(claim => claim.Type == ClaimTypes.Role)
                  .Select(claim => claim.Value)
                  .ToArray();
               return Task.CompletedTask;
            }
         };

      }

      static void SetAuthorizationOptions(AuthorizationOptions options)
      {
         options
            .AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());
      }

   }

   internal static class MongoExtentions
   {

      /*
      internal static bool CheckConnection(this IMongoDatabase mongoDatabase)
      {
         return mongoDatabase.RunCommandAsync((Command<MongoDB.Bson.BsonDocument>)"{ping:1}").Wait(1000);
      }
      */

      /*
      internal static async Task<IMongoCollection<T>> GetCollectionAsync<T>(this IMongoDatabase mongoDatabase, string collectionName)
      {

         // CHECK OF COLLECTION EXISTS
         var options = new ListCollectionNamesOptions { Filter = new MongoDB.Bson.BsonDocument("name", collectionName) };
         var collectionNames = await mongoDatabase.ListCollectionNamesAsync(options);

         // CREATE THE COLLECTION
         if (!await collectionNames.AnyAsync())
            await mongoDatabase.CreateCollectionAsync(collectionName);

         // RETRIEVE AND RETURN THE COLLECTION
         return mongoDatabase.GetCollection<T>(collectionName);

      }
      */

   }
}
