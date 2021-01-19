using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories.Tests
{
   partial class CategoryServiceTests
   {

      [Fact]
      public async void Load_WithNullParameter_MustReturnBadResult()
      {
         var service = CategoryService.Create(null);

         var result = await service.LoadAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(Shared.Results.GetResults("categories", Shared.enResultType.Warning, WARNINGS.INVALID_LOAD_PARAMETER), (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Load_WithValidData_MustReturnOkResultWithData()
      {
         var entity = new CategoryEntity(Shared.EntityID.NewID(), "Category Text", enCategoryType.Income, null);
         var repository = CategoryRepositoryMocker
            .Create()
            .WithLoadCategory(entity)
            .Build();
         var service = CategoryService.Create(repository);

         var result = await service.LoadAsync((string)entity.CategoryID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<CategoryEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (CategoryEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Equal(entity.CategoryID, resultValue.CategoryID);

      }

   }
}
