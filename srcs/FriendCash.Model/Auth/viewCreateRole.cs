#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Auth.Model
{
   public class viewCreateRole
   {

      #region RegEx
      public const string RegexName = "(.{2,100})";
      #endregion

      [Required(ErrorMessage = "MSG_AUTH_ROLE_REQUIRED")]
      [RegularExpression(RegexName, ErrorMessage = "MSG_AUTH_ROLE_INVALID")]
      [Display(Description = "LABEL_AUTH_ROLE")]
      public string Name { get; set; }

   }
}