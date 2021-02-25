using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Balances.Tests
{
   partial class BalanceControllerTests
   {

      [Fact]
      public async void List_MustReturnOkResult_WithDataList()
      {
         var service = BalanceServiceMocker
            .Create()
            .WithList(new OkObjectResult(new BalanceEntity[] { }))
            .Build();
         var controller = new BalanceController(service);

         var date = System.DateTime.Now;
         var result = await controller.ListAsync(date.Year, date.Month);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<BalanceEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (BalanceEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Empty(resultValue);
      }

   }
}
