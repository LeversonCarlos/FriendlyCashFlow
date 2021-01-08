using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void Load_WithValidData_MustReturnOkResultWithData()
      {
         var account = new AccountEntity(new Shared.EntityID(), "Account Text", enAccountType.General, null, null, true);
         var repository = AccountRepositoryMocker
            .Create()
            .WithLoadAccount(account)
            .Build();
         var service = new AccountService(repository);

         var result = await service.LoadAsync(account.AccountID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<AccountEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (AccountEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Equal(account.AccountID, resultValue.AccountID);

      }

   }
}
