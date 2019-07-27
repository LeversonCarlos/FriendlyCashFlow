#region Using
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace FriendCash.Auth.Model
{
   public class bindSignature
   {

      #region New
      internal bindSignature()
      {
         this.RowDate = DateTime.Now;
      }
      #endregion  

      #region idSignature
      [Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idSignature { get; set; }
      #endregion

      #region idUser
      [Column("UserId", TypeName = "varchar")]
      [StringLength(128), Required]
      public string idUser { get; set; }
      #endregion

      #region DueDate
      [Required, DataType(DataType.DateTime)]
      public DateTime DueDate { get; set; }
      #endregion

      #region RowDate
      [DataType(DataType.DateTime)]
      public DateTime RowDate { get; set; }
      #endregion

      #region Status

      [Column("Status")]
      public short StatusValue { get; set; }

      [NotMapped]
      public viewSignature.enumSignatureStatus Status
      {
         get { return ((viewSignature.enumSignatureStatus)this.StatusValue); }
         set { this.StatusValue = ((short)value); }
      }

      #endregion

   }
}