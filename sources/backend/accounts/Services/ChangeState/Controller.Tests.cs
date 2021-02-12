using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountControllerTests
   {

      [Fact]
      public async void ChangeState_WithInvalidParameters_MustReturnBadResult()
      {
         var service = AccountServiceMocker
            .Create()
            .WithChangeState(null, new BadRequestObjectResult(new string[] { WARNINGS.INVALID_CHANGESTATE_PARAMETER }))
            .Build();
         var controller = new AccountController(service);

         var result = await controller.ChangeStateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_CHANGESTATE_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

   }
}
