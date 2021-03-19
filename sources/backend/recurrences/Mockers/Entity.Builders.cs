using System;

namespace Elesse.Recurrences
{
   partial class RecurrenceEntity
   {
      internal static Tests.RecurrenceEntityBuilder Builder() => new Tests.RecurrenceEntityBuilder();
   }
   partial class RecurrenceProperties
   {
      internal static Tests.RecurrencePropertiesBuilder Builder() => new Tests.RecurrencePropertiesBuilder();
   }
}

namespace Elesse.Recurrences.Tests
{

   internal class RecurrenceEntityBuilder
   {

      Shared.EntityID _RecurrenceID = Shared.EntityID.MockerID();
      public RecurrenceEntityBuilder WithRecurrenceID(Shared.EntityID recurrenceID)
      {
         _RecurrenceID = recurrenceID;
         return this;
      }

      IRecurrenceProperties _Properties = RecurrenceProperties.Builder().Build();
      public RecurrenceEntityBuilder WithProperties(IRecurrenceProperties properties)
      {
         _Properties = properties;
         return this;
      }

      public RecurrenceEntity Build() =>
         RecurrenceEntity.Restore(_RecurrenceID, _Properties);

   }

   internal class RecurrencePropertiesBuilder
   {

      Shared.EntityID _PatternID = Shared.EntityID.MockerID();
      public RecurrencePropertiesBuilder WithPatternID(Shared.EntityID patternID)
      {
         _PatternID = patternID;
         return this;
      }

      Shared.EntityID _AccountID = Shared.EntityID.MockerID();
      public RecurrencePropertiesBuilder WithAccountID(Shared.EntityID accountID)
      {
         _AccountID = accountID;
         return this;
      }

      DateTime _Date = Shared.Faker.GetFaker().Date.Soon();
      public RecurrencePropertiesBuilder WithDate(DateTime date)
      {
         _Date = date;
         return this;
      }

      decimal _Value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
      public RecurrencePropertiesBuilder WithExpectedValue(decimal value)
      {
         _Value = value;
         return this;
      }

      enRecurrenceType _Type = Shared.Faker.GetFaker().Random.Enum<enRecurrenceType>();
      public RecurrencePropertiesBuilder WithType(enRecurrenceType type)
      {
         _Type = type;
         return this;
      }

      public RecurrenceProperties Build() =>
         RecurrenceProperties.Create(_PatternID, _AccountID, _Date, _Value, _Type);

   }

}
