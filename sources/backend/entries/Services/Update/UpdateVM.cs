using System;
using Elesse.Patterns;
using Elesse.Shared;

namespace Elesse.Entries
{

   public class UpdateVM
   {

      public EntityID EntryID { get; set; }
      public IPatternEntity Pattern { get; set; }

      public EntityID AccountID { get; set; }
      public DateTime DueDate { get; set; }
      public decimal Value { get; set; }

      public bool Paid { get; set; }
      public DateTime? PayDate { get; set; }

      public UpdateRecurrenceVM Recurrence { get; set; }

   }

   public class UpdateRecurrenceVM : IEntryRecurrenceEntity
   {
      public EntityID RecurrenceID { get; set; }
      public short CurrentOccurrence { get; set; }
      public short TotalOccurrences { get; set; }
   }

}
