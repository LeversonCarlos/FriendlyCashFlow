using System;

namespace Elesse.Entries
{
   public interface IEntryEntity
   {
      Shared.EntityID EntryID { get; }

      Patterns.IPatternEntity Pattern { get; }

      Shared.EntityID AccountID { get; }
      DateTime DueDate { get; }
      decimal EntryValue { get; }

      bool Paid { get; }
      DateTime? PayDate { get; }

      // Shared.EntityID RecurrencyID { get; }
      // short? RecurrencyItem { get; }
      // short? RecurrencyTotal { get; }

      // Shared.EntityID TransferID { get; }

   }
}
