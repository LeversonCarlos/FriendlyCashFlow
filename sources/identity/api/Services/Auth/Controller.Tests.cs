using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class ControllerTests
   {

      [Fact]
      public async void AuthUser_WithNullParameter_MustReturnBadResult()
      {
         var service = ServiceMocker
            .Create()
            .WithAuthUser(null, new BadRequestObjectResult(new string[] { }))
            .Build();
         var controller = new IdentityController(service);

         var result = await controller.AuthUserAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { }, (result as BadRequestObjectResult).Value);
      }

   }
}
