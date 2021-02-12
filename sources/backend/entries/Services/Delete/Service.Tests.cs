using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Delete_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Builder().Build();

         var result = await service.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_DELETE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithInexistingEntry_MustReturnBadRequest()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad()
            .Build();
         var service = EntryService.Builder().With(repository).Build();

         var result = await service.DeleteAsync((string)Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.ENTRY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithInvalidPattern_MustReturnBadResult()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(EntryEntity.Builder().Build())
            .Build();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithDecrease(new ArgumentException(Patterns.WARNINGS.INVALID_DECREASE_PARAMETER))
            .Build();
         var service = EntryService.Mock(repository, patternService);

         var result = await service.DeleteAsync((string)Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(Patterns.WARNINGS.INVALID_DECREASE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithValidParameters_MustReturnOkResult()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(EntryEntity.Builder().Build())
            .Build();
         var service = EntryService.Builder().With(repository).Build();

         var result = await service.DeleteAsync((string)Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
