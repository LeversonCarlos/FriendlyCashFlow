using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Recurrences
{

   internal partial class RecurrenceEntity : IRecurrenceEntity
   {

      Shared.EntityID _RecurrenceID;
      [BsonId]
      public Shared.EntityID RecurrenceID
      {
         get => _RecurrenceID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_RECURRENCEID);
            _RecurrenceID = value;
         }
      }

      IRecurrenceProperties _Properties;
      public IRecurrenceProperties Properties
      {
         get => _Properties;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_PROPERTIES);
            _Properties = value;
         }
      }

      internal void SetProperties(IRecurrenceProperties propertiesParam) =>
         Properties = RecurrenceProperties.Create(propertiesParam.PatternID, propertiesParam.AccountID, propertiesParam.Date, propertiesParam.Value, propertiesParam.Type);

   }

}
