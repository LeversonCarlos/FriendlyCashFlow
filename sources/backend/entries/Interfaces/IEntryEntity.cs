using System;

namespace Elesse.Entries
{
   public interface IEntryEntity
   {
      Shared.EntityID EntryID { get; }

      Patterns.IPatternEntity Pattern { get; }

      Shared.EntityID AccountID { get; }
      DateTime DueDate { get; }
      decimal Value { get; }

      bool Paid { get; }
      DateTime? PayDate { get; }

      IEntryRecurrenceEntity Recurrence { get; }

   }
}
