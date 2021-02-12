using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Categories.Tests
{
   partial class CategoryControllerTests
   {

      [Fact]
      public async void Insert_WithInvalidParameters_MustReturnBadResult()
      {
         var service = CategoryServiceMocker
            .Create()
            .WithInsert(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_INSERT_PARAMETER }))
            .Build();
         var controller = new CategoryController(service);

         var result = await controller.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_INSERT_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
