using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class ControllerTests
   {

      [Fact]
      public async void UserAuth_WithNullParameter_MustReturnBadResult()
      {
         var service = ServiceMocker
            .Create()
            .WithUserAuth(null, new BadRequestObjectResult(new string[] { UserAuthInteractor.WARNING.INVALID_USERAUTH_PARAMETER }))
            .Build();
         var controller = new IdentityController(service);

         var result = await controller.UserAuthAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { UserAuthInteractor.WARNING.INVALID_USERAUTH_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
