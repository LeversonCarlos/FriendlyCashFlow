using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model.Membership
{

   //[DataContract()]
   [Table("webpages_Users")]
   public class UserProfile
   {

      #region idUser
      //[DataMember]
      [Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public int UserId { get; set; }
      #endregion

      #region UserName
      //[DataMember]
      [Display(Description = "Code"), Required(ErrorMessage = "The 'Code' is Required")]
      public string UserName { get; set; }
      #endregion

      #region Description
      //[DataMember]
      [StringLength(500), Display(Description = "Description")]
      public string Description { get; set; }
      #endregion

      //#region Password
      //[IgnoreDataMember]
      //[Column("Password"), StringLength(50), Display(Description = "Password"), Required(ErrorMessage = "The 'Password' is Required")]
      //public string Password { get; set; }
      //#endregion

   }

}
