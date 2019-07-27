#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Auth.Model
{

   public class viewUser
   {
      public string Url { get; set; }

      [Display(Description = "LABEL_USERS_ID")]
      public string ID { get; set; }

      [Display(Description = "LABEL_USERS_USERNAME")]
      public string UserName { get; set; }

      [Display(Description = "LABEL_USERS_FULLNAME")]
      public string FullName { get; set; }

      [Display(Description = "LABEL_USERS_EMAIL")]
      public string Email { get; set; }

      [Display(Description = "LABEL_USERS_EMAILCONFIRMED")]
      public bool EmailConfirmed { get; set; }

      [Display(Description = "LABEL_USERS_JOINDATE")]
      public DateTime JoinDate { get; set; }

      [Display(Description = "LABEL_USERS_EXPIRATIONDATE")]
      public DateTime ExpirationDate { get; set; }

      public viewUserRoles UserRoles { get; set; }
      public IList<string> Roles { get; set; }
      public IList<System.Security.Claims.Claim> Claims { get; set; }

   }

   public class viewUserRoles
   {

      [Display(Description = "LABEL_USERSROLES_ADMIN")]
      public bool Admin { get; set; }

      [Display(Description = "LABEL_USERSROLES_USER")]
      public bool User { get; set; }

      [Display(Description = "LABEL_USERSROLES_VIEWER")]
      public bool Viewer { get; set; }

   }

}