using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class ControllerTests
   {

      [Fact]
      public async void UserAuth_WithNullParameter_MustReturnBadResult()
      {
         var service = ServiceMocker
            .Create()
            .WithUserAuth(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_USERAUTH_PARAMETER }))
            .Build();
         var controller = new IdentityController(service);

         var result = await controller.UserAuthAsync(null);
         
         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_USERAUTH_PARAMETER }, (result.Result as BadRequestObjectResult).Value);
      }

   }
}
