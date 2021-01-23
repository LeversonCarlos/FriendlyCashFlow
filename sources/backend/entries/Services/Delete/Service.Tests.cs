using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Delete_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Mock();

         var result = await service.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_DELETE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithInexistingCategory_MustReturnBadRequest()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad()
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.DeleteAsync((string)Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.ENTRY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithValidParameters_MustReturnOkResult()
      {
         var pattern = Patterns.PatternEntity.Mock();
         var accountID = Shared.EntityID.NewID();
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(EntryEntity.Create(pattern, accountID, System.DateTime.Now, (decimal)12.34))
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.DeleteAsync((string)Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
