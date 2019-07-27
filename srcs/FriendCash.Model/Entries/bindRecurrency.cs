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
   public enum enRecurrencyState : short { Inactive = 0, Active = 1 }
   public enum enRecurrencyType : short { Weekly = 1, Monthly = 2, Bimonthly = 3, Quarterly = 4, SemiYearly = 5, Yearly = 6 }
   public enum enRecurrencyUpdate : short { Current = 0, Futures = 1 }

   [Table("v5_dataRecurrencies")]
   public class bindRecurrency : Base.BaseModel
   {

      #region New
      public bindRecurrency()
      {
         this.idRecurrency = -1;
      }
      #endregion

      #region idRecurrency
      [Column("idRecurrency"), Key]
      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
      public long idRecurrency { get; set; }
      #endregion

      #region idUser
      [Column("idUser", TypeName = "varchar")]
      [StringLength(128), Required]
      [Index("IDX_RECURRENCY_MAIN", Order = 1)]
      public string idUser { get; set; }
      #endregion

      #region idPattern

      [Column("idPattern")]
      public long? idPattern { get; set; }

      // [ForeignKey("idPattern")]
      // public virtual Entries.Model.bindPattern idPatternModel { get; set; }

      #endregion

      #region idAccount

      [Column("idAccount")]
      public long? idAccount { get; set; }

      // [ForeignKey("idAccount")]
      // public virtual Accounts.Model.bindAccount idAccountModel { get; set; }

      #endregion

      #region EntryValue
      [Column("EntryValue"), Required]
      public double EntryValue { get; set; }
      #endregion

      #region EntryDate
      [Column("EntryDate"), Required, DataType(DataType.DateTime)]
      public DateTime EntryDate { get; set; }
      #endregion

      #region Type

      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      public enRecurrencyType Type
      {
         get { return ((enRecurrencyType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }

      #endregion

      #region Fixed
      [Column("Fixed"), Required]
      public bool Fixed { get; set; }
      #endregion

      #region Quantity
      [Column("Quantity")]
      public int? Quantity { get; set; }
      #endregion

      #region InitialDate
      [Column("InitialDate"), Required, DataType(DataType.DateTime)]
      public DateTime InitialDate { get; set; }
      #endregion

      #region FinalDate
      [Column("FinalDate"),  DataType(DataType.DateTime)]
      public DateTime? LastDate { get; set; }
      #endregion

      #region State

      [Column("State"), Required]
      [Index("IDX_RECURRENCY_MAIN", Order = 2)]
      public short StateValue { get; set; }

      [NotMapped]
      public enRecurrencyState State
      {
         get { return ((enRecurrencyState)this.StateValue); }
         set { this.StateValue = ((short)value); }
      }

      #endregion

      #region RowStatus
      [Index("IDX_RECURRENCY_MAIN", Order = 0)]
      [Column("RowStatus")]
      public override short RowStatusValue { get; set; }
      #endregion

      /*
      #region Create

      public static bindPattern Create(viewEntry value)
      {
         return new Model.bindPattern()
         {
            Type = value.Type,
            Text = value.Text,
            idCategory = value.idCategory.Value
         };
      }

      #endregion
      */

   }

}
