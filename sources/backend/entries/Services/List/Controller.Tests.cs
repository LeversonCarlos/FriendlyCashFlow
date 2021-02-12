using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryControllerTests
   {

      [Fact]
      public async void List_MustReturnOkResult_WithDataList()
      {
         var service = EntryServiceMocker
            .Create()
            .WithList(new OkObjectResult(new EntryEntity[] { }))
            .Build();
         var controller = new EntryController(service);

         var dateTime = System.DateTime.Now;
         var result = await controller.ListAsync(dateTime.Year, dateTime.Month);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<EntryEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (EntryEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Empty(resultValue);
      }

   }
}
