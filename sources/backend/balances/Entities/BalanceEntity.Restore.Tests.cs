using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Balances.Tests
{
   partial class BalanceEntityTests
   {

      [Fact]
      public void Restore_WithValidParameters_MustResultInstance()
      {
         var balanceID = Shared.EntityID.MockerID();
         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var expectedValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
         var realizedValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);

         var result = BalanceEntity.Restore(balanceID, accountID, date, expectedValue, realizedValue);

         Assert.NotNull(result);
         Assert.Equal(balanceID, result.BalanceID);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(expectedValue, result.ExpectedValue);
         Assert.Equal(realizedValue, result.RealizedValue);
      }

      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID balanceID, Shared.EntityID accountID, DateTime date, decimal expectedValue, decimal realizedValue)
      {
         var exception = Assert.Throws<ArgumentException>(() => BalanceEntity.Restore(balanceID, accountID, date, expectedValue, realizedValue));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Restore_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_BALANCEID, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_ACCOUNTID, Shared.EntityID.MockerID(), null, null, null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), null, null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), DateTime.MinValue, null, null}
         };

   }
}
