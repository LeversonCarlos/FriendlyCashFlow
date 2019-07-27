#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Auth.Model
{   
   public class viewCreateUser
   {

      #region RegEx
      public const string RegexUsername = "(.{3,100})";
      public const string RegexPassword = "((?=.*\\d)(?=.*[a-z,A-Z])(?=.*[@#$%]).{6,100})";
      // public const string RegexEmail = "([A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,6})";
      /*
         (                 # Start of group
            (?=.*\d)		   #   must contains one digit from 0-9
            (?=.*[a-z])		#   must contains one lowercase characters
            (?=.*[A-Z])		#   must contains one uppercase characters
            (?=.*[@#$%])	#   must contains one special symbols in the list "@#$%"
            .		         #   match anything with previous condition checking
            {6,20}	      #   length at least 6 characters and maximum of 20	
         )			         # End of group
       */
      #endregion

      [Required(ErrorMessage = "MSG_AUTH_EMAIL_REQUIRED")]
      [EmailAddress(ErrorMessage = "MSG_AUTH_EMAIL_INVALID")]
      [Display(Description = "LABEL_AUTH_EMAIL")]
      public string Email { get; set; }

      [Required(ErrorMessage = "MSG_AUTH_FULLNAME_REQUIRED")]
      [Display(Description = "LABEL_AUTH_FULLNAME")]
      public string Fullname { get; set; }

      // [Required(ErrorMessage = "MSG_AUTH_USERNAME_REQUIRED")]
      // [RegularExpression(RegexUsername, ErrorMessage = "MSG_AUTH_USERNAME_INVALID")]
      [Display(Description = "LABEL_AUTH_USERNAME")]
      public string Username { get; set; }

      [DataType(DataType.Password)]
      [Required(ErrorMessage = "MSG_AUTH_PASSWORD_REQUIRED")]
      [RegularExpression(RegexPassword, ErrorMessage = "MSG_AUTH_PASSWORD_INVALID")]
      [Display(Description = "LABEL_AUTH_PASSWORD")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Required(ErrorMessage = "MSG_AUTH_PASSWORD_CONFIRMATION_REQUIRED")]
      [Compare("Password", ErrorMessage = "MSG_AUTH_PASSWORD_CONFIRMATION_INVALID")]
      [Display(Description = "LABEL_AUTH_PASSWORD_CONFIRMATION")]
      public string ConfirmPassword { get; set; }

   }
}