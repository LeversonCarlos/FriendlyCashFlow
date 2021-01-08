using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void List_WithValidData_MustReturnOkResultWithData()
      {
         var account = new AccountEntity(new Shared.EntityID(), "Account Text", enAccountType.General, null, null, true);
         var accountsList = new AccountEntity[] { account };
         var repository = AccountRepositoryMocker
            .Create()
            .WithList(accountsList)
            .Build();
         var service = new AccountService(repository);

         var result = await service.ListAsync();

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