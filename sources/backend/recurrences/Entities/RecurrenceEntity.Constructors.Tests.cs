using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   partial class RecurrenceEntityTests
   {

      [Fact]
      public void Restore_WithValidParameters_MustResultInstance()
      {
         var recurrenceID = Shared.EntityID.MockerID();
         var patternID = Shared.EntityID.MockerID();
         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
         var type = enRecurrenceType.Monthly;

         var result = RecurrenceEntity.Restore(recurrenceID, patternID, accountID, date, value, type);

         Assert.NotNull(result);
         Assert.Equal(recurrenceID, result.RecurrenceID);
         Assert.Equal(patternID, result.PatternID);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(value, result.Value);
         Assert.Equal(type, result.Type);
      }

      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID recurrenceID, Shared.EntityID patternID, Shared.EntityID accountID, DateTime date, decimal value)
      {
         var type = enRecurrenceType.Monthly;
         var exception = Assert.Throws<ArgumentException>(() => RecurrenceEntity.Restore(recurrenceID, patternID, accountID, date, value, type));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Restore_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_RECURRENCEID, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_PATTERNID, Shared.EntityID.MockerID(), null, null, null, null},
            new object[] { WARNINGS.INVALID_ACCOUNTID, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), null, null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), DateTime.MinValue, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.Faker.GetFaker().Date.Soon(), Shared.Faker.GetFaker().Random.Decimal(decimal.MinValue, 0)}
         };

   }
}
