#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Dashboard.Model
{

   public class viewBalance 
   {

      #region idAccount
      [Display(Description = "LABEL_ACCOUNTS_IDACCOUNT")]
      public long idAccount { get; set; }
      #endregion

      #region Text
      [Display(Description = "LABEL_ACCOUNTS_TEXT")]
      public string Text { get; set; }
      #endregion

      #region Type
      [Display(Description = "LABEL_ACCOUNTS_TYPE")]
      public Accounts.Model.enAccountType Type { get; set; }
      #endregion

      #region ClosingDay
      [Display(Description = "LABEL_ACCOUNTS_CLOSINGDAY")]
      public short? ClosingDay { get; set; }
      #endregion

      #region DueDay
      [Display(Description = "LABEL_ACCOUNTS_DUEDAY")]
      public short? DueDay { get; set; }
      #endregion

      #region DueDate
      [Display(Description = "LABEL_REPORTS_ACCOUNTS_DUEDATE")]
      public DateTime? DueDate { get; set; }
      public string DueDateText { get; set; }
      #endregion

      #region InitialBalance
      [Display(Description = "LABEL_ENTRIES_INITIAL_BALANCE")]
      public double InitialBalance { get; set; }
      #endregion

      #region CurrentBalance
      [Display(Description = "LABEL_ENTRIES_CURRENT_BALANCE")]
      public double CurrentBalance { get; set; }
      #endregion

      #region PendingBalance
      [Display(Description = "LABEL_ENTRIES_PENDING_BALANCE")]
      public double PendingBalance { get; set; }
      #endregion

   }

   public class viewBalanceGroup
   {

      #region Text
      [Display(Description = "LABEL_ACCOUNTS_TYPE")]
      public string Text { get; set; }
      #endregion

      #region Type
      [Display(Description = "LABEL_ACCOUNTS_TYPE")]
      public Accounts.Model.enAccountType Type { get; set; }
      #endregion

      #region CurrentBalance
      [Display(Description = "LABEL_ENTRIES_CURRENT_BALANCE")]
      public double CurrentBalance { get; set; }
      #endregion

      #region PendingBalance
      [Display(Description = "LABEL_ENTRIES_PENDING_BALANCE")]
      public double PendingBalance { get; set; }
      #endregion

      #region Accounts
      public List<viewBalance> Accounts { get; set; }
      #endregion  

   }

}