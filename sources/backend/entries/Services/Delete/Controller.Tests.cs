using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryControllerTests
   {

      [Fact]
      public async void Delete_WithInvalidParameters_MustReturnBadResult()
      {
         var service = EntryServiceMocker
            .Create()
            .WithDelete(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_DELETE_PARAMETER }))
            .Build();
         var controller = new EntryController(service);

         var result = await controller.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_DELETE_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
