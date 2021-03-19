using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   partial class RecurrenceServiceTests
   {

      [Fact]
      public async void GetNewRecurrenceAsync_WithInvalidParameter_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();

         var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GetNewRecurrenceAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PROPERTIES, exception.Message);
      }

      /*
      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public async void Increase_WithExistingBalance_MustUpdate_AndReturnChangedBalance(bool paid)
      {
         var param = BalanceEntity.Builder().Build();
         var repository = BalanceRepository
            .Mocker()
            .WithLoad(new IBalanceEntity[] { param })
            .Build();
         var service = BalanceService.Builder().With(repository).Build();

         var value = Shared.Faker.GetFaker().Random.Decimal(-1000, 1000);
         var expectedValue = param.ExpectedValue + value;
         var realizedValue = param.RealizedValue + (paid ? value : 0);

         var result = await service.IncreaseAsync(param.AccountID, param.Date, value, paid);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IBalanceEntity>(result);
         Assert.Equal(param.BalanceID, result.BalanceID);
         Assert.Equal(expectedValue, result.ExpectedValue);
         Assert.Equal(realizedValue, result.RealizedValue);
      }

      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public async void Increase_WithNonExistingBalance_MustCreateRecord_AndReturnChangedBalance(bool paid)
      {
         var repository = BalanceRepository
            .Mocker()
            .WithLoad()
            .Build();
         var service = BalanceService.Builder().With(repository).Build();

         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(-1000, 1000);
         var expectedValue = value;
         var realizedValue = (paid ? value : 0);

         var result = await service.IncreaseAsync(accountID, date, value, paid);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IBalanceEntity>(result);
         Assert.Equal(expectedValue, result.ExpectedValue);
         Assert.Equal(realizedValue, result.RealizedValue);
      }
      */

   }
}
