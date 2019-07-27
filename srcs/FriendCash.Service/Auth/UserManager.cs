#region Using
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
#endregion

namespace FriendCash.Auth
{
   public class AppUserManager : UserManager<Model.bindUser>
   {

      #region New
      public AppUserManager(IUserStore<Model.bindUser> store) : base(store)
      {
      }
      #endregion

      #region Create
      public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
      {
         var appDbContext = context.Get<Model.dbContext>();
         var appUserManager = new AppUserManager(new UserStore<Model.bindUser>(appDbContext));

         // POLICY RULES
         appUserManager.UserValidator = new UserValidator<Model.bindUser>(appUserManager)
         {
            AllowOnlyAlphanumericUserNames = true,
            RequireUniqueEmail = true
         };
         appUserManager.PasswordValidator = new PasswordValidator
         {
            RequiredLength = 6,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = false,
         };

         // EMAIL SERVICE
         appUserManager.EmailService = new AppUserEmailIdentity();
         var dataProtectionProvider = options.DataProtectionProvider;
         if (dataProtectionProvider != null)
         {
            appUserManager.UserTokenProvider = new DataProtectorTokenProvider<Model.bindUser>(dataProtectionProvider.Create("ASP.NET Identity"))
            {
               TokenLifespan = TimeSpan.FromHours(6)
            };
         }

         return appUserManager;
      }
      #endregion

   }

   public class AppUserEmailIdentity : IIdentityMessageService
   {
      public async Task SendAsync(IdentityMessage message)
      { await Service.Configs.Email.SendAsync(message.Subject, message.Body, message.Destination); }
   }

}