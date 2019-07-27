using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Users")]
   public class User: Base
   {

      #region idUser
      [DataMember]
      [Column("idUser"), ConcurrencyCheck]
      public long idUser { get; set; }
      #endregion

      #region Code
      [DataMember]
      [Column("Code"), StringLength(50), ConcurrencyCheck,  Display(Description = "E-Mail"), Required(ErrorMessage = "The 'E-Mail' is Required")]
      public string Code { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description"), StringLength(500), Display(Description = "Name"), Required(ErrorMessage = "The 'Name' is Required")]
      public string Description { get; set; }
      #endregion

      #region Password
      [IgnoreDataMember]
      [Column("Password"), StringLength(50), Display(Description = "Password"), Required(ErrorMessage = "The 'Password' is Required")]
      public string Password { get; set; }
      #endregion

   }

}
