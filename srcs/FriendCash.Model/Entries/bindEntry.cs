#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Entries.Model
{
   public enum enEntryType : short { None = 0, Expense = 1, Income = 2 }

   [Table("v5_dataEntries")]
   public class bindEntry : Base.BaseModel
   {

      #region New
      public bindEntry()
      {
         this.idEntry = -1;
         this.Paid = false;
      }
      #endregion

      #region idEntry
      [Column("idEntry"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idEntry { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      [Index("IDX_ENTRY_TRANSFER", Order = 1)]
      [Index("IDX_ENTRY_MAIN", Order = 1)]
      public string idUser { get; set; }
      #endregion

      #region Text
      [Column("Text", TypeName = "varchar")]
      [StringLength(500), Required]
      public string Text { get; set; }
      #endregion

      #region Type

      [Column("Type"), Required]
      [Index("IDX_ENTRY_MAIN", Order = 2)]
      public short TypeValue { get; set; }

      [NotMapped]
      public enEntryType Type
      {
         get { return ((enEntryType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region idCategory

      [Column("idCategory")]
      public long? idCategory { get; set; }

      [ForeignKey("idCategory")]
      public virtual Categories.Model.bindCategory idCategoryModel { get; set; }

      #endregion

      #region idAccount

      [Column("idAccount")]
      public long? idAccount { get; set; }

      [ForeignKey("idAccount")]
      public virtual Accounts.Model.bindAccount idAccountModel { get; set; }

      #endregion

      #region idTransfer
      [Column("idTransfer", TypeName = "varchar")]
      [StringLength(128)]
      [Index("IDX_ENTRY_TRANSFER", Order = 2)]
      public string idTransfer { get; set; }
      #endregion

      #region idPattern

      [Column("idPattern")]
      public long? idPattern { get; set; }

      [ForeignKey("idPattern")]
      public virtual Entries.Model.bindPattern idPatternModel { get; set; }

      #endregion

      #region idRecurrency

      [Column("idRecurrency")]
      public long? idRecurrency { get; set; }

      [ForeignKey("idRecurrency")]
      public virtual Entries.Model.bindRecurrency idRecurrencyModel { get; set; }

      #endregion

      #region SearchDate
      [Column("SearchDate"), Required, DataType(DataType.DateTime)]
      [Index("IDX_ENTRY_TRANSFER", Order = 3)]
      [Index("IDX_ENTRY_MAIN", Order = 3)]
      public DateTime SearchDate { get; set; }
      #endregion

      #region DueDate
      [Column("DueDate"), Required, DataType(DataType.DateTime)]
      public DateTime DueDate { get; set; }
      #endregion

      #region PayDate
      [Column("PayDate"), DataType(DataType.DateTime)]
      public DateTime? PayDate { get; set; }
      #endregion

      #region Value
      [Column("Value"), Required]
      public double Value { get; set; }
      #endregion

      #region Paid
      [Column("Paid"), Required]
      public bool Paid { get; set; }
      #endregion

      #region Sorting
      [Column("Sorting")]
      public double Sorting { get; set; }
      #endregion

      #region RowStatus
      [Index("IDX_ENTRY_TRANSFER", Order = 0)]
      [Index("IDX_ENTRY_MAIN", Order = 0)]
      [Column("RowStatus")]
      public override short RowStatusValue { get; set; }
      #endregion 


      #region RefreshSorting
      public void RefreshSorting()
      {

         // SEARCH DATE
         this.SearchDate = this.DueDate;
         if (this.Paid && this.PayDate.HasValue) { this.SearchDate = this.PayDate.Value; }

         // SORTING
         double iInterval = Convert.ToInt64(this.SearchDate.Subtract(new DateTime(1901, 1, 1)).TotalDays);
         iInterval += Convert.ToDouble(this.idEntry) / Math.Pow(10, this.idEntry.ToString().Length);
         this.Sorting = iInterval;

      }
      #endregion

   }

}
