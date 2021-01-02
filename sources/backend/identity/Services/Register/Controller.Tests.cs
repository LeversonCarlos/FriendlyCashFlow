using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class ControllerTests
   {

      [Fact]
      public async void Register_WithInvalidParameters_MustReturnBadResult()
      {
         var service = ServiceMocker
            .Create()
            .WithRegister(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_REGISTER_PARAMETER }))
            .Build();
         var controller = new IdentityController(service);

         var result = await controller.RegisterAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_REGISTER_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}