#region Using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace FriendCash.Service.Imports.Model
{

   [Table("v5_dataImportsItem")]
   public class bindImportItem : Base.BaseModel
   {

      #region New
      public bindImportItem()
      {
         this.idImportItem = -1;
      }
      #endregion

      #region idImportItem
      [Column("idImportItem"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idImportItem { get; set; }
      #endregion

      #region idImport

      public long idImport { get; set; }

      [ForeignKey("idImport")]
      public virtual bindImport idImportModel { get; set; }

      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      public string idUser { get; set; }
      #endregion

      #region Type
      public enum enItemType : short { Transfer = 0, Expense = 1, Income = 2 }

      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      public enItemType Type
      {
         get { return ((enItemType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region Date
      [Column("Date")]
      [Required, StringLength(20)] 
      public string Date { get; set; }
      #endregion

      #region Paid
      [Column("Paid")]
      [Required, StringLength(3)]
      public string Paid { get; set; }
      #endregion

      #region Value
      [Column("Value")]
      [Required]
      public decimal Value { get; set; }
      #endregion

      #region Description
      [Column("Description")]
      [Required, StringLength(500)]
      public string Description { get; set; }
      #endregion

      #region Category
      [Column("Category")]
      [StringLength(4000)]
      public string Category { get; set; }
      #endregion

      #region Account
      [Column("Account")]
      [StringLength(500)]
      public string Account { get; set; }
      #endregion

      #region AccountFrom
      [Column("AccountFrom")]
      [StringLength(500)]
      public string AccountFrom { get; set; }
      #endregion

      #region AccountTo
      [Column("AccountTo")]
      [StringLength(500)]
      public string AccountTo { get; set; }
      #endregion

      #region State

      [Column("State"), Required]
      public short StateValue { get; set; }

      [NotMapped]
      public enImportState State
      {
         get { return ((enImportState)this.StateValue); }
         set { this.StateValue = ((short)value); }
      }

      #endregion

   }

}
