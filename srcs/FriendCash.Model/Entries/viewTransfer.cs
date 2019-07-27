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
   public class viewTransfer
   {

      #region New
      public viewTransfer() { }
      #endregion

      #region idTransfer
      [Display(Description = "LABEL_ENTRIES_IDTRANSFER")]
      public string idTransfer { get; set; }
      #endregion

      #region Text
      [StringLength(500, ErrorMessage = "MSG_ENTRIES_TEXT_MAXLENGTH")]
      [Required(ErrorMessage = "MSG_ENTRIES_TEXT_REQUIRED")]
      [Display(Description = "LABEL_ENTRIES_TEXT")]
      public string Text { get; set; }
      #endregion

      #region idEntryIncome
      [Display(Description = "LABEL_ENTRIES_IDENTRYINCOME")]
      public long idEntryIncome { get; set; }
      #endregion

      #region idEntryExpense
      [Display(Description = "LABEL_ENTRIES_IDENTRYEXPENSE")]
      public long idEntryExpense { get; set; }
      #endregion

      #region idAccountIncome
      [Display(Description = "LABEL_ENTRIES_IDACCOUNTINCOME")]
      public long? idAccountIncome { get; set; }
      public virtual Accounts.Model.viewAccount idAccountIncomeView { get; set; }
      #endregion

      #region idAccountExpense
      [Display(Description = "LABEL_ENTRIES_IDACCOUNTEXPENSE")]
      public long? idAccountExpense { get; set; }
      public virtual Accounts.Model.viewAccount idAccountExpenseView { get; set; }
      #endregion

      #region SearchDate
      public DateTime SearchDate { get; set; }
      #endregion

      #region DueDate
      [Required(ErrorMessage = "MSG_ENTRIES_DUEDATE_REQUIRED")]
      [Display(Description = "LABEL_ENTRIES_DUEDATE")]
      public DateTime DueDate { get; set; }
      #endregion

      #region PayDate
      [Display(Description = "LABEL_ENTRIES_PAYDATE")]
      public DateTime? PayDate { get; set; }
      #endregion

      #region Value
      [Required(ErrorMessage = "MSG_ENTRIES_VALUE_REQUIRED")]
      [Display(Description = "LABEL_ENTRIES_VALUE")]
      public double Value { get; set; }
      #endregion

      #region Paid
      [Required(ErrorMessage = "MSG_ENTRIES_PAID_REQUIRED")]
      [Display(Description = "MSG_ENTRIES_PAID")]
      public bool Paid { get; set; }
      #endregion

      #region Sorting
      [Display(Description = "MSG_ENTRIES_SORTING")]
      public double Sorting { get; set; }
      #endregion


      #region Create
      public static viewTransfer Create(bindEntry valueExpense, bindEntry valueIncome)
      {
         var oData = new viewTransfer();
         oData.idTransfer = valueExpense.idTransfer;
         oData.idEntryExpense = valueExpense.idEntry;
         if (valueExpense.idAccount.HasValue)
         {
            oData.idAccountExpense = valueExpense.idAccount;
            if (valueExpense.idAccountModel != null) { oData.idAccountExpenseView = new Accounts.Model.viewAccount(valueExpense.idAccountModel); }
         }
         oData.idEntryIncome = valueIncome.idEntry;
         if (valueIncome.idAccount.HasValue)
         {
            oData.idAccountIncome = valueIncome.idAccount;
            if (valueIncome.idAccountModel != null) { oData.idAccountIncomeView = new Accounts.Model.viewAccount(valueIncome.idAccountModel); }
         }
         oData.SearchDate = valueExpense.SearchDate;
         oData.DueDate = valueExpense.DueDate;
         oData.PayDate = valueExpense.PayDate;
         oData.Value = valueExpense.Value;
         oData.Paid = valueExpense.Paid;
         oData.Sorting = valueExpense.Sorting;

         return oData;
      }
      #endregion

   }
}