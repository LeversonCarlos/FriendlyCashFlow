using Xunit;

namespace Elesse.Categories.Tests
{
   partial class CategoryServiceTests
   {

      [Fact]
      public async void Delete_WithNullParameter_MustReturnBadResult()
      {
         var service = CategoryService.Create(null);

         var result = await service.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Shared.Results.GetResults("categories", Shared.enResultType.Warning, WARNINGS.INVALID_DELETE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithInexistingCategory_MustReturnBadRequest()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithLoadCategory()
            .Build();
         var service = CategoryService.Create(repository);

         var result = await service.DeleteAsync((string)Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Shared.Results.GetResults("categories", Shared.enResultType.Warning, WARNINGS.CATEGORY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithValidParameters_MustReturnOkResult()
      {
         var categoryID = Shared.EntityID.NewID();
         var repository = CategoryRepositoryMocker
            .Create()
            .WithLoadCategory(new CategoryEntity(categoryID, "Category Text", enCategoryType.Income, null))
            .Build();
         var service = CategoryService.Create(repository);

         var result = await service.DeleteAsync((string)categoryID);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
