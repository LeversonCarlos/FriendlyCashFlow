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
      [Column("Code"), StringLength(2000)]
      public string Code { get; set; }
      #endregion

      #region idUser

      [DataMember]
      [Column("idUser"), ConcurrencyCheck]
      public long idUser { get; set; }

      [ForeignKey("idUser")]
      public virtual User UserDetails { get; set; }

      #endregion

   }

 }
