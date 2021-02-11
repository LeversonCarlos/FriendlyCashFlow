using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferControllerTests
   {

      [Fact]
      public async void Update_WithInvalidParameters_MustReturnBadResult()
      {
         var service = TransferServiceMocker
            .Create()
            .WithUpdate(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER }))
            .Build();
         var controller = new TransferController(service);

         var result = await controller.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
