using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   partial class RecurrencePropertiesTests
   {

      [Fact]
      public void Create_WithValidParameters_MustResultInstance()
      {
         var patternID = Shared.EntityID.MockerID();
         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
         var type = enRecurrenceType.Monthly;

         var result = RecurrenceProperties.Create(patternID, accountID, date, value, type);

         Assert.NotNull(result);
         Assert.Equal(patternID, result.PatternID);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(value, result.Value);
         Assert.Equal(type, result.Type);
      }

      [Theory]
      [MemberData(nameof(Create_WithInvalidParameters_MustThrowException_Data))]
      public void Create_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID patternID, Shared.EntityID accountID, DateTime date, decimal value)
      {
         var type = enRecurrenceType.Monthly;
         var exception = Assert.Throws<ArgumentException>(() => RecurrenceProperties.Create(patternID, accountID, date, value, type));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Create_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_PATTERNID, null, null, null, null},
            new object[] { WARNINGS.INVALID_ACCOUNTID, Shared.EntityID.MockerID(), null, null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), DateTime.MinValue, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.Faker.GetFaker().Date.Soon(), Shared.Faker.GetFaker().Random.Decimal(decimal.MinValue, 0)}
         };

   }
}
