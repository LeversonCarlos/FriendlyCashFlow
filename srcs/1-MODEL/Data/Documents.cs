using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_Documents")]
   public class Document : Base
   {

      #region New
      public Document()
      {
         // this.Status = enStatus.Enabled;
       }
      #endregion

      #region idDocument
      [DataMember]
      [Column("idDocument"), Index("IX_DocumentKey")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Document))]
      public long idDocument { get; set; }
      #endregion

      #region Description
      [DataMember]
      [Column("Description", TypeName = "varchar"), StringLength(500)]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DESCRIPTION", ErrorMessageResourceType = typeof(FriendCash.Resources.Document))]
      [Display(Name = "COLUMN_DESCRIPTION", ResourceType = typeof(FriendCash.Resources.Document))]
      public string Description { get; set; }
      #endregion

      #region Type

      [DataMember]
      [Column("Type"), Required]
      public short TypeValue { get; set; }

      [NotMapped]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_TYPE", ErrorMessageResourceType = typeof(FriendCash.Resources.Document))]
      [Display(Name = "COLUMN_TYPE", ResourceType = typeof(FriendCash.Resources.Document))]
      public enType Type
      {
         get { return ((enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
       }
      public enum enType : short { None = 0, Expense = 1, Income = 2, Transfer = 3 }

      #endregion

      #region idSupplier
      [DataMember]
      [Column("idSupplier"), Index("IX_Supplier")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Suppliers))]
      public long? idSupplier { get; set; }
      #endregion

      #region idPlanning
      [DataMember]
      [Column("idPlanning"), Index("IX_Planning")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Planning))]
      public long? idPlanning { get; set; }
      #endregion

      #region Settled
      [DataMember]
      [Column("Settled"), Required]
      [Display(Name = "COLUMN_SETTLED", ResourceType = typeof(FriendCash.Resources.Document))]
      public bool Settled { get; set; }
      #endregion

      #region HystoryCount
      [NotMapped]
      public long HystoryCount { get; set; }
      #endregion

      #region Details

      #region SupplierDetails
      [ForeignKey("idSupplier")] //, Association("FK_DOCUMENTS_SUPPLIER", "idSupplier,RowStatus", "idSupplier,RowStatus")]
      public virtual Supplier SupplierDetails { get; set; }
      #endregion

      #region PlanningDetails
      [ForeignKey("idPlanning")]
      public virtual Planning PlanningDetails { get; set; }
      #endregion 

      #endregion

   }

   #region DocumentIndicators

   public class DocumentIndicators
   {

      #region idDocument
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Document))]
      public long idDocument { get; set; }
      #endregion

      #region HistoryCount
      [Display(Name = "COLUMN_HISTORY_COUNT", ResourceType = typeof(FriendCash.Resources.Document))]
      public int HistoryCount { get; set; }
      #endregion

      #region HistoryUnsettled
      [Display(Name = "COLUMN_HISTORY_UNSETTLED", ResourceType = typeof(FriendCash.Resources.Document))]
      public int HistoryUnsettled { get; set; }
      #endregion

      #region ValueTotal
      [Display(Name = "COLUMN_VALUE_TOTAL", ResourceType = typeof(FriendCash.Resources.Document))]
      public double ValueTotal { get; set; }
      #endregion

      #region ValueSettled
      [Display(Name = "COLUMN_VALUE_SETTLED", ResourceType = typeof(FriendCash.Resources.Document))]
      public double ValueSettled { get; set; }
      #endregion

      #region ValueUnsettled
      [Display(Name = "COLUMN_VALUE_UNSETTLED", ResourceType = typeof(FriendCash.Resources.Document))]
      public double ValueUnsettled { get; set; }
      #endregion

      #region Flow
      public System.Collections.Generic.IEnumerable<MonthlyFlow> Flow { get; set; }
      #endregion

   }

   public class MonthlyFlow
   {

      #region Date
      [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
      public DateTime Date { get; set; }
      #endregion

      #region Value
      public double Value { get; set; }
      #endregion

      #region Average
      public double Average { get; set; }
      #endregion

    }

   #endregion

 }
