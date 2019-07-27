using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   [DataContract()]
   [Table("v4_History")]
   public class History : Base
   {

      #region New
      public History()
      {
         this.Settled = false;
       }
      #endregion

      #region idHistory
      [DataMember]
      [Column("idHistory"), Index("IX_HistoryKey")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.History))]
      public long idHistory { get; set; }
      #endregion

      #region idDocument
      [DataMember]
      [Column("idDocument"), Index("IX_Document")]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DOCUMENT", ErrorMessageResourceType = typeof(FriendCash.Resources.History))]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Document))]
      public long idDocument { get; set; }
      #endregion

      #region Type

      [DataMember]
      [Column("Type")]
      public short TypeValue { get; set; }

      [NotMapped]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_TYPE", ErrorMessageResourceType = typeof(FriendCash.Resources.Document))]
      [Display(Name = "COLUMN_TYPE", ResourceType = typeof(FriendCash.Resources.Document))]
      public Document.enType Type
      {
         get { return ((Document.enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
       }

      #endregion

      #region DueDate
      [DataMember]
      [Column("DueDate")]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DUE_DATE", ErrorMessageResourceType = typeof(FriendCash.Resources.History))]
      [Display(Name = "COLUMN_DUE_DATE", ResourceType = typeof(FriendCash.Resources.History))]
      public DateTime DueDate { get; set; }
      #endregion

      #region Value
      [DataMember]
      [Column("Value")]
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_VALUE", ErrorMessageResourceType = typeof(FriendCash.Resources.History))]
      [Display(Name = "COLUMN_VALUE", ResourceType = typeof(FriendCash.Resources.History))]
      public double Value { get; set; }
      #endregion

      #region Settled
      [DataMember]
      [Column("Settled"), Required]
      [Display(Name = "COLUMN_SETTLED", ResourceType = typeof(FriendCash.Resources.History))]
      public bool Settled { get; set; }       
      #endregion

      #region idAccount
      [DataMember]
      [Column("idAccount"), Index("IX_Account")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Accounts))]
      public long? idAccount { get; set; }
      #endregion

      #region PayDate
      [DataMember]
      [Column("PayDate")]
      [Display(Name = "COLUMN_PAY_DATE", ResourceType = typeof(FriendCash.Resources.History))]
      public DateTime? PayDate { get; set; }
      #endregion

      #region idTransfer
      [DataMember]
      [Column("idTransfer"), Index("IX_Transfer")]
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Transfer))]
      public long? idTransfer { get; set; }
      #endregion

      #region Sorting

      [DataMember]
      [Column("Sorting")]
      [Display(Name = "COLUMN_SORTING", ResourceType = typeof(FriendCash.Resources.History))]
      public double Sorting { get; set; }

      public void SortingRefresh()
      {
         DateTime oInitial = new DateTime(1901, 1, 1);
         DateTime oFinal = (this.Settled == true && this.PayDate.HasValue == true ? this.PayDate.Value : this.DueDate);
         double iInterval = Convert.ToInt64(oFinal.Subtract(oInitial).TotalDays);
         //iInterval = iInterval * 100000;
         iInterval += Convert.ToDouble(this.idHistory) / Math.Pow(10, this.idHistory.ToString().Length);
         this.Sorting = iInterval;
       }

      #endregion

      #region Details

      #region DocumentDetails
      [ForeignKey("idDocument")]
      public virtual Document DocumentDetails { get; set; }
      #endregion

      #region AccountDetails
      [ForeignKey("idAccount")]
      public virtual Account AccountDetails { get; set; }
      #endregion

      #endregion 

   }

}
