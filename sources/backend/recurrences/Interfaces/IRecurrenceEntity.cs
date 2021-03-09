using System;

namespace Elesse.Recurrences
{

   public enum enRecurrenceType : short { Fixed = 0, Weekly = 1, Monthly = 2, Bimonthly = 3, Quarterly = 4, SemiYearly = 5, Yearly = 6 }

   public interface IRecurrenceEntityIdentifier
   {
      Shared.EntityID RecurrenceID { get; }
   }

   public interface IRecurrenceEntityProperties
   {
      Shared.EntityID PatternID { get; }

      Shared.EntityID AccountID { get; }
      DateTime Date { get; }
      decimal Value { get; }

      enRecurrenceType Type { get; set; }
      short Count { get; set; }
   }

   public interface IRecurrenceEntity : IRecurrenceEntityIdentifier, IRecurrenceEntityProperties
   { }

}
