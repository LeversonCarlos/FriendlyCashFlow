using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public void SetRecurrence(IEntryRecurrenceEntity param)
      {
         if (param == null)
            return;
         Recurrence = EntryRecurrenceEntity.Restore(param.RecurrenceID, param.CurrentOccurrence, param.TotalOccurrences);
      }


   }

}
