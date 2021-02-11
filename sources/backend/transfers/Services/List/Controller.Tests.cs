using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferControllerTests
   {

      [Fact]
      public async void List_MustReturnOkResult_WithDataList()
      {
         var service = TransferServiceMocker
            .Create()
            .WithList(new OkObjectResult(new TransferEntity[] { }))
            .Build();
         var controller = new TransferController(service);

         var date = System.DateTime.Now;
         var result = await controller.ListAsync(date.Year, date.Month);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<TransferEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (TransferEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Empty(resultValue);
      }

   }
}
