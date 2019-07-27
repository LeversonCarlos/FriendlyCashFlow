#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Web.Auth.Model
{

   public class viewAuth
   {

      #region New
      public viewAuth() { this.grant_type = "password"; }
      #endregion

      #region username
      [Required(ErrorMessage = "MSG_AUTH_USERNAME_REQUIRED")]
      [Display(Description = "LABEL_AUTH_USERNAME")]
      public string username { get; set; }
      #endregion

      #region password
      [Required(ErrorMessage = "MSG_AUTH_PASSWORD_REQUIRED")]
      [Display(Description = "LABEL_AUTH_PASSWORD")]
      [DataType(DataType.Password)]
      public string password { get; set; }
      #endregion

      #region isPersistent
      [Display(Description = "LABEL_AUTH_ISPERSISTENT")]
      public bool isPersistent { get; set; }
      #endregion

      public string grant_type { get; set; }

   }

}