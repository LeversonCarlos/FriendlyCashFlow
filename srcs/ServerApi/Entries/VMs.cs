using System;

namespace FriendlyCashFlow.API.Entries
{

   public class EntryVM
   {
      public long EntryID { get; set; }

      public Categories.enCategoryType Type { get; set; }
      public string Text { get; set; }

      public long PatternID { get; set; }

      public long CategoryID { get; set; }
      public Categories.CategoryVM CategoryRow { get; set; }
      public string CategoryText { get; set; }

      public DateTime DueDate { get; set; }
      public decimal EntryValue { get; set; }

      public bool Paid { get; set; }
      public DateTime? PayDate { get; set; }
      public long? AccountID { get; set; }
      public string AccountText { get; set; }

      public long? RecurrencyID { get; set; }
      public Recurrencies.RecurrencyVM Recurrency { get; set; }

      public string TransferID { get; set; }
      // public long TransferIncomeEntryID { get; set; }
      // public long TransferExpenseEntryID { get; set; }
      // public long TransferIncomeAccountID { get; set; }
      // public string TransferIncomeAccountText { get; set; }
      // public long TransferExpenseAccountID { get; set; }
      // public string TransferExpenseAccountText { get; set; }

      public decimal BalanceTotalValue { get; set; }
      public decimal BalancePaidValue { get; set; }
      public decimal Sorting { get; set; }

      internal static EntryVM Convert(EntryData value)
      {

         var result = new EntryVM
         {
            EntryID = value.EntryID,
            Type = (Categories.enCategoryType)value.Type,
            Text = value.Text,
            CategoryID = value.CategoryID,
            DueDate = value.DueDate,
            EntryValue = value.EntryValue,
            Paid = value.Paid,
            PayDate = value.PayDate,
            AccountID = value.AccountID
         };

         return result;
      }


   }

   public class EntryFlowVM
   {
      public string Day { get; set; }
      public EntryVM[] EntryList { get; set; }
      public decimal BalanceTotalValue { get; set; }
      public decimal BalancePaidValue { get; set; }
   }

}
