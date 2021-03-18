using System;
using System.Collections.Generic;

namespace Elesse.Recurrences
{
   internal partial class RecurrenceProperties : Shared.ValueObject, IRecurrenceProperties
   {

      Shared.EntityID _PatternID;
      public Shared.EntityID PatternID
      {
         get => _PatternID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_PATTERNID);
            _PatternID = value;
         }
      }

      Shared.EntityID _AccountID;
      public Shared.EntityID AccountID
      {
         get => _AccountID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_ACCOUNTID);
            _AccountID = value;
         }
      }

      DateTime _Date;
      public DateTime Date
      {
         get => _Date;
         private set
         {
            if (value == null || value == DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_DATE);
            _Date = value;
         }
      }

      decimal _Value;
      public decimal Value
      {
         get => _Value;
         private set
         {
            if (value <= 0)
               throw new ArgumentException(WARNINGS.INVALID_VALUE);
            _Value = value;
         }
      }

      enRecurrenceType _Type;
      public enRecurrenceType Type
      {
         get => _Type;
         private set
         {
            _Type = value;
         }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return PatternID;
         yield return AccountID;
         yield return Date;
         yield return Value;
         yield return Type;
      }

   }
}
