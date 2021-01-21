using System;

namespace Elesse.Entries
{

   public enum enEntryType : short { Expense = 1, Income = 2 }

   public interface IEntryEntity
   {
      Shared.EntityID EntryID { get; }
      enEntryType Type { get; }

      Shared.EntityID AccountID { get; }
      // public DateTime SearchDate { get; set; }

      string Text { get; }
      Shared.EntityID CategoryID { get; }
      Shared.EntityID PatternID { get; }

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
