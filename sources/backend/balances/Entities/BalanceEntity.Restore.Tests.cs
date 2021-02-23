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

      /*
      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID transferID, Shared.EntityID expenseAccountID, Shared.EntityID incomeAccountID, DateTime date, decimal value)
      {
         var exception = Assert.Throws<ArgumentException>(() => TransferEntity.Restore(transferID, expenseAccountID, incomeAccountID, date, value));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Restore_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_TRANSFERID, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_EXPENSEACCOUNTID, Shared.EntityID.MockerID(), null, null, null, null},
            new object[] { WARNINGS.INVALID_INCOMEACCOUNTID, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), null, null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), DateTime.MinValue, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.Faker.GetFaker().Date.Soon(), null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.Faker.GetFaker().Date.Soon(), 0},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.EntityID.MockerID(), Shared.Faker.GetFaker().Date.Soon(), Shared.Faker.GetFaker().Random.Decimal(decimal.MinValue, (decimal)(0-0.01))}
         };
      */

   }
}
