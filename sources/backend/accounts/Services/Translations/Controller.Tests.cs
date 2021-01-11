using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountControllerTests
   {

      /*
      [Fact]
      public async void List_MustReturnOkResult_WithDataList()
      {
         var service = AccountServiceMocker
            .Create()
            .WithList(new OkObjectResult(new AccountEntity[] { }))
            .Build();
         var controller = new AccountController(service);

         var result = await controller.ListAsync();

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<AccountEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (AccountEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Empty(resultValue);
      }
      */

   }
}
