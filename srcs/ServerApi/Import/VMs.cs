using System;
using System.Collections.Generic;

namespace FriendlyCashFlow.API.Import
{

   public class ImportVM
   {
      public bool ClearDataBefore { get; set; }
      public List<EntryVM> Entries { get; set; }
      public List<TransferVM> Transfers { get; set; }
      internal string ResourceID { get; set; }
      internal List<Accounts.AccountVM> Accounts { get; set; }
      internal List<Categories.CategoryVM> Categories { get; set; }
   }

   public class EntryVM
   {

      public Categories.enCategoryType Type { get; set; }
      public string Text { get; set; }

      internal long? AccountID { get; set; }
      public string Account { get; set; }

      internal long? CategoryID { get; set; }
      public string Category { get; set; }

      public DateTime DueDate { get; set; }
      public decimal Value { get; set; }

      public bool Paid { get; set; }
      public DateTime? PayDate { get; set; }

      internal long? RecurrencyID { get; set; }
      public string Recurrency { get; set; }

   }

   public class TransferVM
   {
      public DateTime Date { get; set; }
      public decimal Value { get; set; }
      internal long? IncomeAccountID { get; set; }
      public string IncomeAccount { get; set; }
      internal long? ExpenseAccountID { get; set; }
      public string ExpenseAccount { get; set; }
   }

}
