using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Shared.Tests
{
   partial class ResultsTests
   {

      [Fact]
      public void Ok_WithData_MustResultOkObjectResult()
      {
         var param = EntityID.MockerID();

         var result = Results.Ok(param);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result);
      }

      [Fact]
      public void Ok_WithoutData_MustResultOkResult()
      {

         var result = Results.Ok();

         Assert.NotNull(result);
         Assert.IsType<OkResult>(result);
      }

   }
}
