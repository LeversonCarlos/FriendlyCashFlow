using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Elesse.Balances.Tests
{
   partial class BalanceServiceTests
   {

      [Theory]
      [MemberData(nameof(Increase_WithInvalidParameter_MustThrowException_Data))]
      public async void Increase_WithInvalidParameter_MustThrowException(string exceptionText, Shared.EntityID accountID, DateTime date, decimal value, bool paid)
      {
         var service = BalanceService.Builder().Build();

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.IncreaseAsync(accountID, date, value, paid));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Increase_WithInvalidParameter_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_ACCOUNTID, null, null, 0, false },
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), null, 0, false },
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.MockerID(), DateTime.MinValue, 0, false }
         };


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

   }
}
