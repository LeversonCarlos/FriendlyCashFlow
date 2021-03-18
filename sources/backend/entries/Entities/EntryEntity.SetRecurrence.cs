using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public void SetRecurrence(IEntryRecurrenceEntity recurrence)
      {
         Recurrence = recurrence;
      }

   }

}
