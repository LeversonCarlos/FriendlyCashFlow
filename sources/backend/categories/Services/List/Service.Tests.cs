using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories.Tests
{
   partial class CategoryServiceTests
   {

      [Fact]
      public async void List_WithValidData_MustReturnOkResultWithData()
      {
         var entity = new CategoryEntity(Shared.EntityID.NewID(), "Category Text", enCategoryType.Income, null);
         var entityList = new CategoryEntity[] { entity };
         var repository = CategoryRepositoryMocker
            .Create()
            .WithList(entityList)
            .Build();
         var service = CategoryService.Create(repository);

         var result = await service.ListAsync();

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<CategoryEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (CategoryEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Single(resultValue);
         Assert.Equal(entity.CategoryID, resultValue[0].CategoryID);

      }

   }
}
