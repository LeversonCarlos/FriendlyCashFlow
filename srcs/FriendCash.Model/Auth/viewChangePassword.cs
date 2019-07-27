#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Auth.Model
{
   public class viewChangePassword
   {

      [DataType(DataType.Password)]
      [Required(ErrorMessage = "MSG_AUTH_PASSWORD_CURRENT_REQUIRED")]
      [Display(Description = "LABEL_AUTH_PASSWORD_CURRENT")]
      public string OldPassword { get; set; }

      [DataType(DataType.Password)]
      [Required(ErrorMessage = "MSG_AUTH_PASSWORD_NEW_REQUIRED")]
      [RegularExpression(viewCreateUser.RegexPassword, ErrorMessage = "MSG_AUTH_PASSWORD_INVALID")]
      [Display(Description = "LABEL_AUTH_PASSWORD_NEW")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Required(ErrorMessage = "MSG_AUTH_PASSWORD_CONFIRMATION_REQUIRED")]
      [Compare("NewPassword", ErrorMessage = "MSG_AUTH_PASSWORD_CONFIRMATION_INVALID")]
      [Display(Description = "LABEL_AUTH_PASSWORD_CONFIRMATION")]
      public string ConfirmPassword { get; set; }

   }
}