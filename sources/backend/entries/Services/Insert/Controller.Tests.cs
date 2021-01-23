using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryControllerTests
   {

      [Fact]
      public async void Insert_WithInvalidParameters_MustReturnBadResult()
      {
         var service = EntryServiceMocker
            .Create()
            .WithInsert(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_INSERT_PARAMETER }))
            .Build();
         var controller = new EntryController(service);

         var result = await controller.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_INSERT_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
