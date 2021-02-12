using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void Search_WithValidData_MustReturnOkResultWithData()
      {
         var account = new AccountEntity(Shared.EntityID.NewID(), "Account Text", enAccountType.General, null, null, true);
         var accountsList = new AccountEntity[] { account };
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts(accountsList)
            .Build();
         var service = AccountService.Create(repository);

         var result = await service.SearchAsync(account.Text);

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
