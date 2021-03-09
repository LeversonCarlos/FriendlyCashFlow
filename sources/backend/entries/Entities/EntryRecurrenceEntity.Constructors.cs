using System;

namespace Elesse.Entries
{

   partial class EntryRecurrenceEntity
   {

      private EntryRecurrenceEntity() { }

      public static EntryRecurrenceEntity Restore(Shared.EntityID recurrenceID, short currentOccurrence, short totalOccurrences) =>
         new EntryRecurrenceEntity
         {
            RecurrenceID = recurrenceID,
            CurrentOccurrence = currentOccurrence,
            TotalOccurrences = totalOccurrences
         };

      /*
      public static EntryRecurrenceEntity Create(short currentOccurrence, short totalOccurrences) =>
         new EntryRecurrenceEntity
         {
            RecurrenceID = Shared.EntityID.NewID(),
            CurrentOccurrence = currentOccurrence,
            TotalOccurrences = totalOccurrences
         };
      */

   }

}
