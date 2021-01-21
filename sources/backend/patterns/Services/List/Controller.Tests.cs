using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternControllerTests
   {

      [Fact]
      public async void List_MustReturnOkResult_WithDataList()
      {
         var service = PatternServiceMocker
            .Create()
            .WithList(new OkObjectResult(new PatternEntity[] { }))
            .Build();
         var controller = new PatternController(service);

         var result = await controller.ListAsync();

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<PatternEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (PatternEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Empty(resultValue);
      }

   }
}
