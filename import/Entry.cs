using System;

namespace Import
{

   public enum enEntryType { None = 0, Expense = 1, Income = 2 };
   public enum enRecurrencyType { Fixed = 0, Weekly = 1, Monthly = 2, Bimonthly = 3, Quarterly = 4, SemiYearly = 5, Yearly = 6 }

   public class Entry
   {
      public enEntryType Type { get; set; }
      public string Text { get; set; }

      public string Category { get; set; }
      public long? CategoryID { get; set; }

      public DateTime DueDate { get; set; }
      public decimal EntryValue { get; set; }

      public bool Paid { get; set; }
      public DateTime? PayDate { get; set; }

      public long? AccountID { get; set; }
      public string Account { get; set; }

      public long? RecurrencyID { get; set; }
      public enRecurrencyType? RecurrencyType { get; set; }
      public int? RecurrencyCount { get; set; }

   }
}
