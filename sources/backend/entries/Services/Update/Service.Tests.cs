using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Update_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Mock();

         var result = await service.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_UPDATE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInexistingEntry_MustReturnBadRequest()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad()
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.UpdateAsync(new UpdateVM { EntryID = Shared.EntityID.NewID() });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.ENTRY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithExceptionWhenChangingPattern_MustReturnBadRequest()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(EntryEntity.Mock())
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.UpdateAsync(new UpdateVM { EntryID = Shared.EntityID.NewID() });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(Patterns.WARNINGS.INVALID_INCREASE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInvalidAccount_MustReturnBadRequest()
      {
         var entity = EntryEntity.Mock();
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.UpdateAsync(new UpdateVM { EntryID = Shared.EntityID.NewID(), Pattern = entity.Pattern });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_ACCOUNTID), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }
      /*
      [Fact]
      public async void Insert_WithInvalidPattern_MustReturnBadResult()
      {
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(new ArgumentException(Patterns.WARNINGS.INVALID_INCREASE_PARAMETER))
            .Build();
         var service = EntryService.Mock(patternService);

         var result = await service.InsertAsync(new InsertVM { });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(Patterns.WARNINGS.INVALID_INCREASE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithInvaidAccount_MustReturnBadResult()
      {
         var pattern = Patterns.PatternEntity.Mock();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(pattern)
            .Build();
         var service = EntryService.Mock(patternService);

         var param = new InsertVM { Pattern = pattern, AccountID = null };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_ACCOUNTID), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithValidParameters_MustReturnOkResult()
      {
         var pattern = Patterns.PatternEntity.Mock();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(pattern)
            .Build();
         var service = EntryService.Mock(patternService);

         var param = new InsertVM { Pattern = pattern, AccountID = Shared.EntityID.NewID(), DueDate = DateTime.Now.AddDays(1), EntryValue = (decimal)12.34 };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }
      */

   }
}
