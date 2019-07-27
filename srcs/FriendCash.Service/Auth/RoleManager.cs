#region Using
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Auth
{
   public class AppRoleManager : RoleManager<IdentityRole>
   {

      #region New
      public AppRoleManager(IRoleStore<IdentityRole, string> roleStore) : base(roleStore)
      {
      }
      #endregion

      #region Create
      public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
      {
         var appRoleManager = new AppRoleManager(new RoleStore<IdentityRole>(context.Get<Model.dbContext>()));

         return appRoleManager;
      }
      #endregion

   }
}