using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Load_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Create(null);

         var result = await service.LoadAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(Warning(WARNINGS.INVALID_LOAD_PARAMETER), (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Load_WithValidData_MustReturnOkResultWithData()
      {
         var entity = EntryEntity.Create(Patterns.PatternEntity.Mock(), Shared.EntityID.NewID(), DateTime.Now.AddDays(10), (decimal)54.32);
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = EntryService.Create(repository);

         var result = await service.LoadAsync((string)entity.EntryID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<EntryEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (EntryEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Equal(entity.EntryID, resultValue.EntryID);

      }

   }
}
