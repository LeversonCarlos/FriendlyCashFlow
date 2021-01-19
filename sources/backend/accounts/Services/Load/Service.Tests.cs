using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void Load_WithNullParameter_MustReturnBadResult()
      {
         var service = AccountService.Create(null);

         var result = await service.LoadAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(Shared.Results.GetResults("accounts", Shared.enResultType.Warning, WARNINGS.INVALID_LOAD_PARAMETER), (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Load_WithValidData_MustReturnOkResultWithData()
      {
         var account = new AccountEntity(Shared.EntityID.NewID(), "Account Text", enAccountType.General, null, null, true);
         var repository = AccountRepositoryMocker
            .Create()
            .WithLoadAccount(account)
            .Build();
         var service = AccountService.Create(repository);

         var result = await service.LoadAsync((string)account.AccountID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<AccountEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (AccountEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Equal(account.AccountID, resultValue.AccountID);

      }

   }
}
