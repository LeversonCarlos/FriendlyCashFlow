using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class ControllerTests
   {

      [Fact]
      public async void TokenAuth_WithNullParameter_MustReturnBadResult()
      {
         var service = ServiceMocker
            .Create()
            .WithTokenAuth(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER }))
            .Build();
         var controller = new IdentityController(service);

         var result = await controller.TokenAuthAsync(null);
         
         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER }, (result.Result as BadRequestObjectResult).Value);
      }

   }
}
