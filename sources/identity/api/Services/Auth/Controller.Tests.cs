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
            .WithUserAuth(null, new BadRequestObjectResult(new string[] { }))
            .Build();
         var controller = new IdentityController(service);

         var result = await controller.UserAuthAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { }, (result as BadRequestObjectResult).Value);
      }

   }
}
