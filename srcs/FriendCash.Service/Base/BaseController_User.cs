#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
#endregion

namespace FriendCash.Service.Base
{
   partial class BaseController
   {

      #region UserManager
      private Auth.AppUserManager _UserManager = null;
      internal Auth.AppUserManager UserManager
      {
         get
         { return this._UserManager ?? Request.GetOwinContext().GetUserManager<Auth.AppUserManager>(); }
      }
      #endregion

      #region RoleManager
      private Auth.AppRoleManager _RoleManager = null;
      internal Auth.AppRoleManager RoleManager
      {
         get
         { return this._RoleManager ?? Request.GetOwinContext().GetUserManager<Auth.AppRoleManager>(); }
      }
      #endregion

      #region GetUserID
      protected string GetUserID()
      { return User.Identity.GetUserId(); }
      #endregion

   }
}