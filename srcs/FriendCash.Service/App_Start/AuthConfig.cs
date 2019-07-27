#region Using
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace FriendCash.Service
{
   partial class Startup
   {

      #region ConfigureAuth
      internal void ConfigureAuth(IAppBuilder app)
      {
         HttpConfiguration httpConfig = new HttpConfiguration();
         this.ConfigureAuth_OAuthToken(app);
         this.ConfigureAuth_Seed(app);
         Configs.Route.Register(httpConfig);
         app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
         app.UseWebApi(httpConfig);
      }
      #endregion

      #region ConfigureAuth_OAuthToken

      private void ConfigureAuth_OAuthToken(IAppBuilder app)
      {
         var issuerPath = new PathString("/").ToUriComponent(); /* ConfigurationManager.AppSettings["general:BaseAddress"] */
         var audienceID = ConfigurationManager.AppSettings["auth:AudienceID"];
         var audienceSecretValue = ConfigurationManager.AppSettings["auth:AudienceSecret"];
         byte[] audienceSecret = Microsoft.Owin.Security.DataHandler.Encoder.TextEncodings.Base64Url.Decode(audienceSecretValue);

         this.ConfigureAuth_OAuthToken_Generate(app, issuerPath, audienceID, audienceSecret);
         this.ConfigureAuth_OAuthToken_Consumption(app, issuerPath, audienceID, audienceSecret);
       }

      private void ConfigureAuth_OAuthToken_Generate(IAppBuilder app, string issuerPath, string audienceID, byte[] audienceSecret)
      {

         // CONTEXT AND USER MANAGER 
         app.CreatePerOwinContext(Auth.Model.dbContext.Create);
         app.CreatePerOwinContext<Auth.AppUserManager>(Auth.AppUserManager.Create);
         app.CreatePerOwinContext<Auth.AppRoleManager>(Auth.AppRoleManager.Create);

         // OAUTH BEARER JSON WEB TOKEN 
         var OAuthServerOptions = new OAuthAuthorizationServerOptions()
         {
            AllowInsecureHttp = true, /* (on production should be AllowInsecureHttp = false) */
            TokenEndpointPath = new PathString("/oauth/token"),
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5),
            Provider = new Configs.Provider.OAuth(),
            RefreshTokenProvider = new Configs.Provider.OAuthRefresh(), 
            AccessTokenFormat = new Configs.Provider.TokenFormat(issuerPath, audienceID, audienceSecret)
            //, RefreshTokenFormat = new Configs.Provider.TokenFormat(issuerPath, audienceID, audienceSecret)
         };
         app.UseOAuthAuthorizationServer(OAuthServerOptions);
         // app.UseOAuthBearerTokens(OAuthServerOptions);
      }

      private void ConfigureAuth_OAuthToken_Consumption(IAppBuilder app, string issuerPath, string audienceID, byte[] audienceSecret)
      {
         app.UseJwtBearerAuthentication(
               new JwtBearerAuthenticationOptions
               {
                  AuthenticationMode = AuthenticationMode.Active,
                  AllowedAudiences = new[] { audienceID }, 
                  IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuerPath, audienceSecret)
                    }
               });
      }

      #endregion

      #region ConfigureAuth_Seed

      private void ConfigureAuth_Seed(IAppBuilder app)
      {
         try
         {

            // MANAGER
            var dbContext = Auth.Model.dbContext.Create();
            var userManager = new UserManager<Auth.Model.bindUser>(new UserStore<Auth.Model.bindUser>(dbContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            // DATA
            this.ConfigureAuth_Seed_Clients(app, dbContext);
            this.ConfigureAuth_Seed_Roles(app, roleManager);
            this.ConfigureAuth_Seed_User(app, userManager);

         }
         catch (Exception ex) { /*throw ex;*/ }
      }

      private void ConfigureAuth_Seed_Clients(IAppBuilder app, Auth.Model.dbContext authContext)
      {
         try
         {

            this.ConfigureAuth_Seed_Client(app, authContext, "*******",
               new Auth.Model.bindClient()
               {
                  Id = "debugWebApp",
                  Name = "Debug Web Application",
                  Type = Auth.Model.viewClient.enumType.NativeConfidential,
                  RefreshTokenLifetime = 60 * 5, /* 5hours */
                  AllowedOrigin = "http://localhost:6583/"
               });

         }
         catch (Exception ex) { throw ex; }
      }

      private void ConfigureAuth_Seed_Client(IAppBuilder app, Auth.Model.dbContext authContext, string secret, Auth.Model.bindClient client) {
         try
         {
            if (authContext.Clients.Count(x=> x.Id == client.Id) == 0)
            {
               client.Secret = Model.Base.Hash.Execute(secret);
               client.Active = true;
               authContext.Clients.Add(client);
               authContext.SaveChanges();
            }
         }
         catch (Exception ex) { throw ex; }
      }

      private void ConfigureAuth_Seed_Roles(IAppBuilder app, RoleManager<IdentityRole> roleManager)
      {
         if (roleManager.Roles.Count() == 0)
         {
            roleManager.Create(new IdentityRole { Name = "Admin" });
            roleManager.Create(new IdentityRole { Name = "User" });
            roleManager.Create(new IdentityRole { Name = "Viewer" });
         }
      }

      private void ConfigureAuth_Seed_User(IAppBuilder app, UserManager<Auth.Model.bindUser> userManager)
      {
         if (userManager.Users.Count() == 0)
         {

            // USER
            var user = new Auth.Model.bindUser()
            {
               UserName = "sysadmin",
               FullName = "Administrator", 
               Email = "name@email.com.br",
               JoinDate = DateTime.UtcNow,
               ExpirationDate = DateTime.UtcNow.AddYears(10),
               EmailConfirmed = true
            };
            userManager.Create(user, "*******");

            // ROLES
            user = userManager.FindByName("sysadmin");
            userManager.AddToRoles(user.Id, new string[] { "Admin", "User" });

         }
      }

      #endregion

   }
}