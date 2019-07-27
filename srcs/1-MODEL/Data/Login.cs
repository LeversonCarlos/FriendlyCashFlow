using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract]
   [Table("v4_Logins")]
   public class Login : Base
   {

      #region Code
      [DataMember]
      [Column("idLogin")]
      public string Code { get; set; }
      #endregion

      #region idUser
      [DataMember]
      [Column("idUser")]
      public string idUser { get; set; }
      #endregion

      #region ConnStr
      [NotMapped]
      public string ConnStr { get; set; }
      #endregion

   }

 }
