using System;
using Elesse.Patterns;
using Elesse.Shared;

namespace Elesse.Entries
{
   public class InsertVM
   {

      public IPatternEntity Pattern { get; set; }

      public EntityID AccountID { get; set; }
      public DateTime DueDate { get; set; }
      public decimal Value { get; set; }

      public bool Paid { get; set; }
      public DateTime? PayDate { get; set; }

   }
}
