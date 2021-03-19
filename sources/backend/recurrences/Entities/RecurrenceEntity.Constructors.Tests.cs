using System;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   partial class RecurrenceEntityTests
   {


      [Fact]
      public void Create_WithValidParameters_MustResultInstance()
      {
         var patternID = Shared.EntityID.MockerID();
         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
         var type = enRecurrenceType.Monthly;
         var properties = RecurrenceProperties.Create(patternID, accountID, date, value, type);

         var result = RecurrenceEntity.Create(properties);

         Assert.NotNull(result);
         Assert.Equal(patternID, result.Properties.PatternID);
         Assert.Equal(accountID, result.Properties.AccountID);
         Assert.Equal(date, result.Properties.Date);
         Assert.Equal(value, result.Properties.Value);
         Assert.Equal(type, result.Properties.Type);
      }

      [Fact]
      public void Create_WithInvalidParameters_MustThrowException()
      {
         RecurrenceProperties properties = null;

         var exception = Assert.Throws<ArgumentException>(() => RecurrenceEntity.Create(properties));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PROPERTIES, exception.Message);
      }

   }
}
