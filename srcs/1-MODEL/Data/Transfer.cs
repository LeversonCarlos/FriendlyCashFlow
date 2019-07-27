using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FriendCash.Model
{

   public class Transfer : Model.Base
   {

      #region idTransfer
      public long idTransfer { get; set; }
      #endregion

      #region idDocument
      public long idDocument { get; set; }
      #endregion

      #region DueDate
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_DUE_DATE", ErrorMessageResourceType = typeof(FriendCash.Resources.Transfer))]
      [Display(Name = "COLUMN_DUE_DATE", ResourceType = typeof(FriendCash.Resources.Transfer))]
      public DateTime DueDate { get; set; }
      #endregion

      #region Value
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_VALUE", ErrorMessageResourceType = typeof(FriendCash.Resources.Transfer))]
      [Display(Name = "COLUMN_VALUE", ResourceType = typeof(FriendCash.Resources.Transfer))]
      public double Value { get; set; }
      #endregion

      #region Settled
      public bool Settled { get; set; }
      #endregion

      #region idAccountIncome
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_ACCOUNT_INCOME", ErrorMessageResourceType = typeof(FriendCash.Resources.Transfer))]
      [Display(Name = "COLUMN_ACCOUNT_INCOME", ResourceType = typeof(FriendCash.Resources.Transfer))]
      public long? idAccountIncome { get; set; }
      #endregion

      #region idAccountExpense
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_ACCOUNT_EXPENSE", ErrorMessageResourceType = typeof(FriendCash.Resources.Transfer))]
      [Display(Name = "COLUMN_ACCOUNT_EXPENSE", ResourceType = typeof(FriendCash.Resources.Transfer))]
      public long? idAccountExpense { get; set; }
      #endregion

      #region idRowIncome
      public Int64 idRowIncome { get; set; }
      #endregion

      #region idRowExpense
      public Int64 idRowExpense { get; set; }
      #endregion

      #region PayDate
      [Required(ErrorMessageResourceName = "MSG_REQUIRED_PAY_DATE", ErrorMessageResourceType = typeof(FriendCash.Resources.Transfer))]
      [Display(Name = "COLUMN_PAY_DATE", ResourceType = typeof(FriendCash.Resources.Transfer))]
      public DateTime? PayDate { get; set; }
      #endregion

      #region Details

      #region DocumentDetails
      [ForeignKey("idDocument")]
      public virtual Document DocumentDetails { get; set; }
      #endregion

      #region AccountIncomeDetails
      [ForeignKey("idAccountIncome")]
      public virtual Account AccountIncomeDetails { get; set; }
      #endregion

      #region AccountExpenseDetails
      [ForeignKey("idAccountExpense")]
      public virtual Account AccountExpenseDetails { get; set; }
      #endregion

      #endregion 

   }

 }
