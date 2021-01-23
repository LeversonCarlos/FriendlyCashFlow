using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryControllerTests
   {

      /*
      [Fact]
      public async void List_MustReturnOkResult_WithDataList()
      {
         var service = CategoryServiceMocker
            .Create()
            .WithList(new OkObjectResult(new CategoryEntity[] { }))
            .Build();
         var controller = new CategoryController(service);

         var result = await controller.ListAsync();

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<CategoryEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (CategoryEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Empty(resultValue);
      }
      */

   }
}
