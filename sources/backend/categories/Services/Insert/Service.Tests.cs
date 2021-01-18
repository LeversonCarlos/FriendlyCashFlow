using Xunit;

namespace Elesse.Categories.Tests
{
   partial class CategoryServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameter_MustReturnBadResult()
      {
         var service = CategoryService.Create(null);

         var result = await service.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_INSERT_PARAMETER }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      /*
      [Fact]
      public async void Insert_WithExistingText_MustReturnBadRequest()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts(null, new AccountEntity[] { new AccountEntity("Category Text", enAccountType.General, null, null) })
            .Build();
         var service = CategoryService.Create(repository);
         var param = new InsertVM { Text = "Category Text", Type = enAccountType.General };

         var result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);

         result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.ACCOUNT_TEXT_ALREADY_USED }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }
      */

      [Fact]
      public async void Insert_WithValidParameters_MustReturnOkResult()
      {
         var repository = CategoryRepositoryMocker
            .Create()
            // .WithSearchAccounts()
            .Build();
         var service = CategoryService.Create(repository);

         var param = new InsertVM { Text = "Category Text", Type = enCategoryType.Income };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
