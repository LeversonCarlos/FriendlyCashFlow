#region Using
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Auth.Model
{
   internal class Convert
   {

      #region viewRole
      internal static viewRole ToView(IdentityRole appRole, Service.Base.BaseController baseController)
      {
         return new viewRole
         {
            Url = baseController.UrlHelper.Link("GetRoleByID", new { id = appRole.Id }),
            ID = appRole.Id,
            Name = appRole.Name
         };
      }
      #endregion

      #region viewUser
      internal static viewUser ToView(bindUser appUser, Service.Base.BaseController baseController)
      {

         // INITIALIZE
         var userResult = new viewUser
         {
            Url = baseController.UrlHelper.Link("GetUserByID", new { id = appUser.Id }),
            ID = appUser.Id,
            UserName = appUser.UserName,
            FullName = appUser.FullName,
            Email = appUser.Email,
            EmailConfirmed = appUser.EmailConfirmed,
            JoinDate = appUser.JoinDate,
            ExpirationDate = appUser.ExpirationDate,
            Roles = baseController.UserManager.GetRolesAsync(appUser.Id).Result,
            Claims = baseController.UserManager.GetClaimsAsync(appUser.Id).Result
         };

         // USER ROLES
         userResult.UserRoles = new viewUserRoles();
         userResult.UserRoles.Admin = userResult.Roles.Count(x => x == "Admin") != 0;
         userResult.UserRoles.User = userResult.Roles.Count(x => x == "User") != 0;
         userResult.UserRoles.Viewer = userResult.Roles.Count(x => x == "Viewer") != 0;

         // RESULT
         return userResult;
      }
      #endregion

      #region viewClient
      internal static viewClient ToView(bindClient value, Service.Base.BaseController baseController)
      {
         return new viewClient
         {
            Url = baseController.UrlHelper.Link("GetClientByID", new { id = value.Id }),
            ID = value.Id,
            Name = value.Name,
            Type = value.Type,
            RefreshTokenLifetime = value.RefreshTokenLifetime,
            AllowedOrigin = value.AllowedOrigin,
            Active = value.Active
         };
      }
      #endregion

      #region viewSignature
      internal static viewSignature ToView(bindSignature value, Service.Base.BaseController baseController)
      {
         return new viewSignature
         {
            idSignature = value.idSignature,
            Status = value.Status,
            DueDate = value.DueDate,
            RowDate = value.RowDate
         };
      }
      #endregion

   }
}