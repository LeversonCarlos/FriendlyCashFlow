using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elesse.Balances.Tests
{
   partial class BalanceServiceTests
   {

      [Fact]
      public async void List_WithValidData_MustReturnOkResultWithData()
      {
         var date = Shared.Faker.GetFaker().Date.Soon();
         var entity = BalanceEntity.Builder().WithDate(date).Build();
         var repository = BalanceRepositoryMocker
            .Create()
            .WithList(new BalanceEntity[] { entity })
            .Build();
         var service = BalanceService.Builder().With(repository).Build();

         var result = await service.ListAsync(date.Year, date.Month);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<BalanceEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (BalanceEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Single(resultValue);
         Assert.Equal(entity.BalanceID, resultValue[0].BalanceID);

      }

   }
}
