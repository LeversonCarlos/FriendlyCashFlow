using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountControllerTests
   {

      [Fact]
      public async void Load_MustReturnOkResult_WithDataList()
      {
         var account = new AccountEntity("Account Text", enAccountType.General, null, null);
         var service = AccountServiceMocker
            .Create()
            .WithLoad(account.AccountID, new OkObjectResult(account))
            .Build();
         var controller = new AccountController(service);

         var result = await controller.LoadAsync((string)account.AccountID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<AccountEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (AccountEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
      }

   }
}
