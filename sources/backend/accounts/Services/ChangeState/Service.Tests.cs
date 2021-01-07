using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void ChangeState_WithNullParameter_MustReturnBadResult()
      {
         var service = new AccountService(null);

         var result = await service.ChangeStateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_CHANGESTATE_PARAMETER }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void ChangeState_WithInexistingAccount_MustReturnBadRequest()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithGetAccountByID()
            .Build();
         var service = new AccountService(repository);
         var param = new ChangeStateVM { AccountID = new Shared.EntityID(), State = false };

         var result = await service.ChangeStateAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.ACCOUNT_NOT_FOUND }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void ChangeState_WithValidParameters_MustReturnOkResult()
      {
         var accountID = new Shared.EntityID();
         var repository = AccountRepositoryMocker
            .Create()
            .WithGetAccountByID(new AccountEntity(accountID, "Account Text", enAccountType.General, null, null, true))
            .Build();
         var service = new AccountService(repository);

         var param = new ChangeStateVM { AccountID = accountID, State = false };
         var result = await service.ChangeStateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
