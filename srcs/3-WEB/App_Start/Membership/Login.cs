using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model.Membership
{

   #region LocalLogin
   public class LocalLogin
   {
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_USERNAME", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_USERNAME", ResourceType = typeof(FriendCash.Resources.User))]
      public string UserName { get; set; }

      [DataType(DataType.Password)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_PASSWORD", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_PASSWORD", ResourceType = typeof(FriendCash.Resources.User))]
      public string Password { get; set; }

      [Display(Name = "COLUMN_REMEMBER", ResourceType = typeof(FriendCash.Resources.User))]
      public bool RememberMe { get; set; }

   }
   #endregion

   #region LocalPassword
   public class LocalPassword
   {
      [DataType(DataType.Password)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_PASSWORD_CURRENT", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_PASSWORD_CURRENT", ResourceType = typeof(FriendCash.Resources.User))]
      public string OldPassword { get; set; }

      [DataType(DataType.Password)]
      [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "MSG_WARNING_PASSWORD", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_PASSWORD_NEW", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_PASSWORD_NEW", ResourceType = typeof(FriendCash.Resources.User))]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Compare("NewPassword", ErrorMessageResourceName = "MSG_WARNING_PASSWORD_CONFIRM", ErrorMessageResourceType = typeof(FriendCash.Resources.User))]
      [Display(Name = "COLUMN_PASSWORD_CONFIRM", ResourceType = typeof(FriendCash.Resources.User))]
      public string ConfirmPassword { get; set; }
   }
   #endregion

   #region ServiceLogin
   public class ServiceLogin
   {
      public string Provider { get; set; }
      public string ProviderDisplayName { get; set; }
      public string ProviderUserId { get; set; }
   }
   #endregion

}