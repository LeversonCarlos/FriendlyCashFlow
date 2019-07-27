using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Accounts")]
   public class Account : Base
   {

      #region New
      public Account()
      {
         this.Status = enStatus.Enabled;
         this.Type = enType.None;
       }
      #endregion

      #region idAccount
      [DataMember]
      [Column("idAccount"), ConcurrencyCheck, Display(Description = "ID")]
      public long idAccount { get; set; }
      #endregion

      #region Code
      [DataMember]
      [Column("Code"), StringLength(50), Required, Display(Description = "Code")]
      public string Code { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description"), StringLength(500), Required, Display(Description = "Description")]
      public string Description { get; set; }
      #endregion

      #region Type

      [DataMember]
      [Column("Type"), Required]
      public short TypeValue { get; set; }

      public enum enType : short { None = 0, Bank = 1, CreditCard = 2 }

      [NotMapped]
      public enType Type
      {
         get { return ((enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region DueDay
      [DataMember]
      [Column("DueDay")]
      public short? DueDay { get; set; }
      #endregion

      #region Status

      [DataMember]
      [Column("Status")]
      public short StatusValue { get; set; }

      public enum enStatus : short { Disabled = 0, Enabled = 1 }

      [NotMapped]
      public enStatus Status
      {
         get { return ((enStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }

      #endregion

   }

 }
