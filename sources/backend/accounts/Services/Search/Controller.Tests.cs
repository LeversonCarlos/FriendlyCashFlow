using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountControllerTests
   {

      [Fact]
      public async void Search_MustReturnOkResult_WithDataList()
      {
         var account = new AccountEntity("Account Text", enAccountType.General, null, null);
         var accountsList = new AccountEntity[] { account };
         var service = AccountServiceMocker
            .Create()
            .WithSearch(account.Text, new OkObjectResult(accountsList))
            .Build();
         var controller = new AccountController(service);

         var result = await controller.SearchAsync(account.Text);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<AccountEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (AccountEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Single(resultValue);
         Assert.Equal(account.AccountID, resultValue[0].AccountID);
      }

   }
}
