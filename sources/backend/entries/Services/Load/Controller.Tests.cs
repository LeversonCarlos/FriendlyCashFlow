using Microsoft.AspNetCore.Mvc;
using Xunit;
using System;

namespace Elesse.Entries.Tests
{
   partial class EntryControllerTests
   {

      [Fact]
      public async void Load_MustReturnOkResult_WithDataList()
      {
         var entity = EntryEntity.Create(Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.Now.AddDays(10), (decimal)54.32);
         var service = EntryServiceMocker
            .Create()
            .WithLoad((string)entity.EntryID, new OkObjectResult(entity))
            .Build();
         var controller = new EntryController(service);

         var result = await controller.LoadAsync((string)entity.EntryID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<EntryEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (EntryEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
      }

   }
}
