using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Elesse.Balances.Tests
{
   partial class BalanceServiceTests
   {

      [Theory]
      [MemberData(nameof(Decrease_WithInvalidParameter_MustThrowException_Data))]
      public async void Decrease_WithInvalidParameter_MustThrowException(string exceptionText, Shared.EntityID accountID, DateTime date)
      {
         var service = BalanceService.Builder().Build();

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.DecreaseAsync(accountID, date, 0, false));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Decrease_WithInvalidParameter_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_ACCOUNTID, null, null },
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), null },
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), DateTime.MinValue }
         };


      [Theory]
      [InlineData(false)]
      [InlineData(true)]
      public async void Decrease_WithExistingBalance_MustUpdate_AndReturnChangedBalance(bool paid)
      {
         var param = BalanceEntity.Builder().Build();
         var repository = BalanceRepository
            .Mocker()
            .WithLoad(new IBalanceEntity[] { param })
            .Build();
         var service = BalanceService.Builder().With(repository).Build();

         var value = Shared.Faker.GetFaker().Random.Decimal(-1000, 1000);
         var expectedValue = param.ExpectedValue - value;
         var realizedValue = param.RealizedValue - (paid ? value : 0);

         var result = await service.DecreaseAsync(param.AccountID, param.Date, value, paid);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IBalanceEntity>(result);
         Assert.Equal(param.BalanceID, result.BalanceID);
         Assert.Equal(expectedValue, result.ExpectedValue);
         Assert.Equal(realizedValue, result.RealizedValue);
      }

      [Fact]
      public async void Decrease_WithNonExistingBalance_MustResultNull()
      {
         var repository = BalanceRepository
            .Mocker()
            .WithLoad()
            .Build();
         var service = BalanceService.Builder().With(repository).Build();

         var accountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();

         var result = await service.DecreaseAsync(accountID, date, 0, false);

         Assert.Null(result);
      }

   }
}
