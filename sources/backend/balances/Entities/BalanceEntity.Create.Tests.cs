using System;
using Xunit;

namespace Elesse.Balances.Tests
{
   partial class BalanceEntityTests
   {

      [Fact]
      public void Create_WithValidParameters_MustResultInstance()
      {
         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var expectedValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
         var realizedValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);

         var result = BalanceEntity.Create(accountID, date, expectedValue, realizedValue);

         Assert.NotNull(result);
         Assert.NotNull(result.BalanceID);
         Assert.Equal(36, ((string)result.BalanceID).Length);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(expectedValue, result.ExpectedValue);
         Assert.Equal(realizedValue, result.RealizedValue);
      }

   }
}
