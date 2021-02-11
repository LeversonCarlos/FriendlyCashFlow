using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;

namespace Elesse.Transfers.Tests
{
   partial class TransferControllerTests
   {

      [Fact]
      public async void Load_MustReturnOkResult_WithDataList()
      {
         var entity = TransferEntity.Builder().Build();
         var service = TransferServiceMocker
            .Create()
            .WithLoad((string)entity.TransferID, new OkObjectResult(entity))
            .Build();
         var controller = new TransferController(service);

         var result = await controller.LoadAsync((string)entity.TransferID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<TransferEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (TransferEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
      }

   }
}
