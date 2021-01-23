using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryControllerTests
   {

      [Fact]
      public async void Update_WithInvalidParameters_MustReturnBadResult()
      {
         var service = EntryServiceMocker
            .Create()
            .WithUpdate(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER }))
            .Build();
         var controller = new EntryController(service);

         var result = await controller.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
