using System;

namespace Elesse.Entries
{

   partial class EntryEntity
   {

      public void Change(Patterns.IPatternEntity pattern, Shared.EntityID accountID, DateTime dueDate, decimal entryValue)
      {
         Pattern = pattern;
         AccountID = accountID;
         DueDate = dueDate;
         EntryValue = entryValue;
      }

   }

}
