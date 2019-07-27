#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Entries.Model
{
   public class viewEntry
   {
      public enum enEntryState : short { None = 0, Paid = 1, Overdue = 2 }

      #region New
      public viewEntry() { }
      #endregion

      #region idEntry
      [Display(Description = "LABEL_ENTRIES_IDENTRY")]
      public long idEntry { get; set; }
      #endregion

      #region Text
      [StringLength(500, ErrorMessage = "MSG_ENTRIES_TEXT_MAXLENGTH")]
      [Required(ErrorMessage = "MSG_ENTRIES_TEXT_REQUIRED")]
      [Display(Description = "LABEL_ENTRIES_TEXT")]
      public string Text { get; set; }
      #endregion

      #region Type
      public short TypeValue { get; set; }
      [Display(Description = "LABEL_ENTRIES_TYPE")]
      public enEntryType Type
      {
         get { return ((enEntryType)this.TypeValue); }
         set { this.TypeValue = ((short)value); }
      }
      #endregion

      #region idCategory
      [Display(Description = "LABEL_CATEGORIES_IDCATEGORY")]
      public long? idCategory { get; set; }
      public virtual Categories.Model.viewCategory idCategoryView { get; set; }
      public string idCategoryText { get; set; }
      #endregion

      #region idAccount
      [Display(Description = "LABEL_ACCOUNTS_IDACCOUNT")]
      public long? idAccount { get; set; }
      public virtual Accounts.Model.viewAccount idAccountView { get; set; }
      public string idAccountText { get; set; }
      #endregion

      #region idPattern
      [Display(Description = "LABEL_ENTRIES_IDPATTERN")]
      public long? idPattern { get; set; }
      #endregion

      #region SearchDate
      [DataType(DataType.DateTime)]
      public DateTime SearchDate { get; set; }

      public bool SearchDateFuture
      {
         get
         {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var searchDate = new DateTime(this.SearchDate.Year, this.SearchDate.Month, this.SearchDate.Day);
            return (searchDate > now);
         }
      }
      #endregion

      #region DueDate
      [Required(ErrorMessage = "MSG_ENTRIES_DUEDATE_REQUIRED")]
      [Display(Description = "LABEL_ENTRIES_DUEDATE")]
      [DataType(DataType.DateTime)]
      public DateTime DueDate { get; set; }
      #endregion

      #region PayDate
      [Display(Description = "LABEL_ENTRIES_PAYDATE")]
      [DataType(DataType.DateTime)]
      public DateTime? PayDate { get; set; }
      #endregion

      #region EntryValue
      [Required(ErrorMessage = "MSG_ENTRIES_VALUE_REQUIRED")]
      [Display(Description = "LABEL_ENTRIES_VALUE")]
      public double EntryValue { get; set; }
      #endregion

      #region Paid
      [Required(ErrorMessage = "MSG_ENTRIES_PAID_REQUIRED")]
      [Display(Description = "MSG_ENTRIES_PAID")]
      public bool Paid { get; set; }
      #endregion

      #region State
      public enEntryState State { get; set; }
      #endregion

      #region Balance
      [Display(Description = "LABEL_ENTRIES_BALANCE")]
      public double Balance { get; set; }
      #endregion

      #region Sorting
      [Display(Description = "MSG_ENTRIES_SORTING")]
      public double Sorting { get; set; }
      #endregion

      #region RowDate
      public DateTime RowDate { get; set; }
      #endregion


      #region idTransfer
      [Display(Description = "LABEL_ENTRIES_IDTRANSFER")]
      public string idTransfer { get; set; }
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
      public string idAccountIncomeText { get; set; }
      #endregion

      #region idAccountExpense
      [Display(Description = "LABEL_ENTRIES_IDACCOUNTEXPENSE")]
      public long? idAccountExpense { get; set; }
      public virtual Accounts.Model.viewAccount idAccountExpenseView { get; set; }
      public string idAccountExpenseText { get; set; }
      #endregion


      #region idRecurrency
      public long? idRecurrency { get; set; }
      public virtual Entries.Model.viewRecurrency idRecurrencyView { get; set; }
      #endregion


      #region Create
      public static viewEntry Create(bindEntry entryValue)
      {
         // var entryResult = Activator.CreateInstance<T>();
         var entryResult = new viewEntry();
         entryResult.idEntry = entryValue.idEntry;
         entryResult.Text = entryValue.Text;
         entryResult.idTransfer = entryValue.idTransfer;
         entryResult.Type = entryValue.Type;
         entryResult.SearchDate = entryValue.SearchDate;
         entryResult.DueDate = entryValue.DueDate;
         entryResult.PayDate = entryValue.PayDate;
         entryResult.EntryValue = entryValue.Value;
         entryResult.Paid = entryValue.Paid;
         entryResult.Sorting = entryValue.Sorting;

         // PATTERN
         if (entryValue.idPattern.HasValue)
         { entryResult.idPattern = entryValue.idPattern; }

         // RECURRENCY
         entryResult.idRecurrency = entryValue.idRecurrency;
         entryResult.idRecurrencyView = new Entries.Model.viewRecurrency();
         if (entryValue.idRecurrencyModel != null) { entryResult.idRecurrencyView = new Entries.Model.viewRecurrency(entryValue.idRecurrencyModel); }

         // CATEGORY
         entryResult.idCategory = entryValue.idCategory;
         if (entryValue.idCategoryModel != null) { entryResult.idCategoryView = new Categories.Model.viewCategory(entryValue.idCategoryModel); }

         // ACCOUNT
         if (entryValue.idAccount.HasValue)
         {
            entryResult.idAccount = entryValue.idAccount;
            if (entryValue.idAccountModel != null) { entryResult.idAccountView = new Accounts.Model.viewAccount(entryValue.idAccountModel); }
         }

         return entryResult;
      }
      #endregion

   }
}