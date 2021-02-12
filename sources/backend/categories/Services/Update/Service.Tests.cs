using Xunit;

namespace Elesse.Categories.Tests
{
   partial class CategoryServiceTests
   {

      [Fact]
      public async void Update_WithNullParameter_MustReturnBadResult()
      {
         var service = CategoryService.Create();

         var result = await service.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_UPDATE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInexistingParentCategory_MustReturnBadRequest()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithLoadCategory()
            .Build();
         var service = CategoryService.Create(repository);
         var param = new UpdateVM { CategoryID = Shared.EntityID.NewID(), Text = "Category Text", Type = enCategoryType.Income, ParentID = Shared.EntityID.NewID() };

         var result = await service.UpdateAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.PARENT_CATEGORY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithExistingText_MustReturnBadRequest()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithSearchCategories(new CategoryEntity[] { new CategoryEntity(Shared.EntityID.NewID(), "Category Text", enCategoryType.Income, null) })
            .Build();
         var service = CategoryService.Create(repository);
         var param = new UpdateVM { CategoryID = Shared.EntityID.NewID(), Text = "Category Text", Type = enCategoryType.Income };

         var result = await service.UpdateAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.CATEGORY_TEXT_ALREADY_USED), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInexistingCategory_MustReturnBadRequest()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithSearchCategories()
            .WithLoadCategory()
            .Build();
         var service = CategoryService.Create(repository);
         var param = new UpdateVM { CategoryID = Shared.EntityID.NewID(), Text = "Category Text", Type = enCategoryType.Income };

         var result = await service.UpdateAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.CATEGORY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithValidParameters_MustReturnOkResult()
      {
         var categoryID = Shared.EntityID.NewID();
         var repository = CategoryRepositoryMocker
            .Create()
            .WithSearchCategories()
            .WithLoadCategory(new CategoryEntity(categoryID, "Category Text", enCategoryType.Income, null))
            .Build();
         var service = CategoryService.Create(repository);

         var param = new UpdateVM { CategoryID = categoryID, Text = "Changed Category Text", Type = enCategoryType.Income };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
