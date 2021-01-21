using System;

namespace Elesse.Entries
{

   public enum enEntryType : short { Expense = 1, Income = 2 }

   public interface IEntryEntity
   {
      Shared.EntityID EntryID { get; }

      Shared.EntityID AccountID { get; }
      // public DateTime SearchDate { get; set; }

      Patterns.IPatternEntity Pattern { get; }

      DateTime DueDate { get; }
      decimal EntryValue { get; }

      bool Paid { get; }
      DateTime? PayDate { get; }

      Shared.EntityID RecurrencyID { get; }
      short? RecurrencyItem { get; }
      short? RecurrencyTotal { get; }

      Shared.EntityID TransferID { get; }

      decimal Sorting { get; }
   }

}
