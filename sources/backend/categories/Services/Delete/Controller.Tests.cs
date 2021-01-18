using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Categories.Tests
{
   partial class CategoryControllerTests
   {

      [Fact]
      public async void Delete_WithInvalidParameters_MustReturnBadResult()
      {
         var service = CategoryServiceMocker
            .Create()
            .WithDelete(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_DELETE_PARAMETER }))
            .Build();
         var controller = new CategoryController(service);

         var result = await controller.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_DELETE_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
