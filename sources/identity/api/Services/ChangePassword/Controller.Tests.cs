using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class ControllerTests
   {

      [Fact]
      public async void ChangePassword_WithNullParameter_MustReturnBadResult()
      {
         var identityService = ServiceMocker
            .Create()
            .WithChangePassword(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER }))
            .Build();
         var controller = new IdentityController(identityService);
         controller.ControllerContext = ControllerHelper.GetControllerContext("my-user-id");

         var result = await controller.ChangePasswordAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
