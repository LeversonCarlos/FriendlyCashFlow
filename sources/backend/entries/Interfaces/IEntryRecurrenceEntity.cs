using System;

namespace Elesse.Entries
{
   public interface IEntryRecurrenceEntity
   {
      Shared.EntityID RecurrenceID { get; }
      short CurrentOccurrence { get; }
      short TotalOccurrences { get; }
   }
}
