using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model.Membership
{

   #region LocalRegister
   public class LocalRegister
   {

      [Required(ErrorMessageResourceName = "MSG_REQUIRED_USERNAME", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_USERNAME", ResourceType = typeof(FriendCash.Resources.User))]
      public string UserName { get; set; }

      [DataType(DataType.Password)]
      [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "MSG_WARNING_PASSWORD", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_PASSWORD", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_PASSWORD", ResourceType = typeof(FriendCash.Resources.User))]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Compare("Password", ErrorMessageResourceName = "MSG_WARNING_PASSWORD_CONFIRM", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_PASSWORD_CONFIRM", ResourceType = typeof(FriendCash.Resources.User))]
      public string ConfirmPassword { get; set; }
   }
   #endregion

   #region ExternalRegister
   public class ExternalRegister
   {
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_USERNAME", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_USERNAME", ResourceType = typeof(FriendCash.Resources.User))]
      public string UserName { get; set; }

      public string ExternalLoginData { get; set; }
   }
   #endregion

}