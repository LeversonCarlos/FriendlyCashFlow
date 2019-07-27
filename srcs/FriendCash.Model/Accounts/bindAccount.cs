#region Using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace FriendCash.Service.Accounts.Model
{

   [Table("v5_dataAccounts")]
   public class bindAccount : Base.BaseModel
   {

      #region New
      public bindAccount()
      {
         this.idAccount = -1;
         this.Active = true;
         this.Type = enAccountType.General;
      }
      #endregion

      #region idAccount
      [Column("idAccount"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idAccount { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      public string idUser { get; set; }
      #endregion

      #region Text
      [Column("Text", TypeName = "varchar")]
      [StringLength(500), Required]
      public string Text { get; set; }
      #endregion

      #region Type

      [Column("Type")]
      public short TypeValue { get; set; }

      [NotMapped]
      public enAccountType Type
      {
         get { return ((enAccountType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region ClosingDay
      [Column("ClosingDay")]
      public short? ClosingDay { get; set; }
      #endregion

      #region DueDay
      [Column("DueDay")]
      public short? DueDay { get; set; }
      #endregion

      #region Active
      [Column("Active")]
      public bool Active { get; set; }
      #endregion

   }

}
