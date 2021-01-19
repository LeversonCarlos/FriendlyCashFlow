using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void Update_WithNullParameter_MustReturnBadResult()
      {
         var service = AccountService.Create();

         var result = await service.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Shared.Results.GetResults("accounts", Shared.enResultType.Warning, WARNINGS.INVALID_UPDATE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInvalidType_MustReturnBadResult()
      {
         var service = AccountService.Create();

         var param = new UpdateVM { AccountID = Shared.EntityID.NewID(), Text = "Account Text", Type = enAccountType.Bank, ClosingDay = 1, DueDay = 1 };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Shared.Results.GetResults("accounts", Shared.enResultType.Warning, WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithExistingText_MustReturnBadRequest()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts(new AccountEntity[] { new AccountEntity(Shared.EntityID.NewID(), "Account Text", enAccountType.General, null, null, true) })
            .Build();
         var service = AccountService.Create(repository);
         var param = new UpdateVM { AccountID = Shared.EntityID.NewID(), Text = "Account Text", Type = enAccountType.General };

         var result = await service.UpdateAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Shared.Results.GetResults("accounts", Shared.enResultType.Warning, WARNINGS.ACCOUNT_TEXT_ALREADY_USED), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInexistingAccount_MustReturnBadRequest()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts()
            .WithLoadAccount()
            .Build();
         var service = AccountService.Create(repository);
         var param = new UpdateVM { AccountID = Shared.EntityID.NewID(), Text = "Account Text", Type = enAccountType.General };

         var result = await service.UpdateAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Shared.Results.GetResults("accounts", Shared.enResultType.Warning, WARNINGS.ACCOUNT_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithValidParameters_MustReturnOkResult()
      {
         var accountID = Shared.EntityID.NewID();
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts()
            .WithLoadAccount(new AccountEntity(accountID, "Account Text", enAccountType.General, null, null, true))
            .Build();
         var service = AccountService.Create(repository);

         var param = new UpdateVM { AccountID = accountID, Text = "Changed Account Text", Type = enAccountType.General };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
