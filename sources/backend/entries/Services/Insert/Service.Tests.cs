using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Mock();

         var result = await service.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_INSERT_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithInvalidPattern_MustReturnBadResult()
      {
         var service = EntryService.Mock();

         var result = await service.InsertAsync(new InsertVM { });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_PATTERN), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      /*
      [Fact]
      public async void Insert_WithNullParameter_MustReturnBadResult()
      {
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .Build();
         var service = EntryService.Mock(patternService);

         var result = await service.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_INSERT_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }
      */

      /*
      [Fact]
      public async void Insert_WithInexistingParentCategory_MustReturnBadRequest()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithLoadCategory()
            .Build();
         var service = CategoryService.Create(repository);
         var param = new InsertVM { Text = "Category Text", Type = enCategoryType.Income, ParentID = Shared.EntityID.NewID() };

         var result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.PARENT_CATEGORY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithExistingText_MustReturnBadRequest()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithSearchCategories(null, new CategoryEntity[] { new CategoryEntity("Category Text", enCategoryType.Income, null) })
            .Build();
         var service = CategoryService.Create(repository);
         var param = new InsertVM { Text = "Category Text", Type = enCategoryType.Income };

         var result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);

         result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.CATEGORY_TEXT_ALREADY_USED), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithValidParameters_MustReturnOkResult()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            .WithSearchCategories()
            .Build();
         var service = CategoryService.Create(repository);

         var param = new InsertVM { Text = "Category Text", Type = enCategoryType.Income };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }
      */

   }
}
