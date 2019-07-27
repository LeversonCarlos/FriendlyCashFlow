using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   #region Flow
   public class Flow
   {

      #region idHistory
      public long idHistory { get; set; }
      #endregion

      #region idDocument
      public long idDocument { get; set; }
      #endregion

      #region idTransfer
      public long? idTransfer { get; set; }
      #endregion

      #region InnerDescription
      public string InnerDescription { get; set; }
      #endregion

      #region Description
      [Display(Name = "COLUMN_ID", ResourceType = typeof(FriendCash.Resources.Document))]
      public string Description { get { return this.InnerDescription; } }
      #endregion

      #region Settled
      [Display(Description = "Settled")]
      public bool Settled { get; set; }
      #endregion

      #region DueDate
      [Display(Name = "COLUMN_DUE_DATE", ResourceType = typeof(FriendCash.Resources.Flow))]
      public DateTime DueDate { get; set; }
      #endregion

      #region PayDate
      public DateTime? PayDate { get; set; }
      #endregion

      #region Date
      [Display(Description = "Date")]
      public DateTime Date { get { return (this.Settled == true && this.PayDate.HasValue == true ? this.PayDate.Value : this.DueDate); } }
      #endregion

      #region ParcelQuantity
      public int ParcelQuantity { get; set; }
      #endregion

      #region ParcelItem
      public int ParcelItem { get; set; }
      #endregion

      #region Type

      public short TypeValue { get; set; }

      [Display(Description = "Type")]
      [NotMapped]
      public Document.enType Type
      {
         get { return ((Document.enType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
       }

      #endregion

      #region InnerValue
      public double InnerValue { get; set; }
      #endregion

      #region Value
      [Display(Name = "COLUMN_VALUE", ResourceType = typeof(FriendCash.Resources.Flow))]
      public double Value { get { return this.InnerValue * (this.Type == Document.enType.Expense ? -1 : 1); } }
      #endregion

      #region idAccount
      [Display(Description = "Account")]
      public long? idAccount { get; set; }
      #endregion

      #region BalanceActual
      [Display(Name = "COLUMN_BALANCE_CURRENT", ResourceType = typeof(FriendCash.Resources.Flow))]
      public double? BalanceActual { get; set; }
      #endregion

      #region BalanceProvided
      [Display(Name = "COLUMN_BALANCE_PROVIDED", ResourceType = typeof(FriendCash.Resources.Flow))]
      public double BalanceProvided { get; set; }
      #endregion

      #region Sorting
      public double Sorting { get; set; }
      #endregion

   }
   #endregion

   #region ViewMonthlyFlow
   public class ViewMonthlyFlow
   {

      #region Date
      public DateTime Date { get; set; }
      #endregion

      #region IncomeValue
      public double IncomeValue { get; set; }
      #endregion

      #region ExpenseValue
      public double ExpenseValue { get; set; }
      #endregion

      #region BalanceValue
      public double BalanceValue { get ; set; }
      #endregion

    }
   #endregion
}
