using System;

namespace Elesse.Entries
{
   public interface IEntryRecurrenceEntity
   {
      Shared.EntityID RecurrencyID { get; }
      short CurrentOccurrence { get; }
      short TotalOccurrences { get; }
   }
}
