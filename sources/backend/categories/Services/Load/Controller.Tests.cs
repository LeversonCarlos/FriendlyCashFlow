using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Categories.Tests
{
   partial class CategoryControllerTests
   {

      [Fact]
      public async void Load_MustReturnOkResult_WithDataList()
      {
         var entity = new CategoryEntity("Category Text", enCategoryType.Income, null);
         var service = CategoryServiceMocker
            .Create()
            .WithLoad((string)entity.CategoryID, new OkObjectResult(entity))
            .Build();
         var controller = new CategoryController(service);

         var result = await controller.LoadAsync((string)entity.CategoryID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<CategoryEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (CategoryEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
      }

   }
}
