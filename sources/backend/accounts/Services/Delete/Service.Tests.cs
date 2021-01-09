using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void Delete_WithNullParameter_MustReturnBadResult()
      {
         var service = new AccountService(null);

         var result = await service.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_DELETE_PARAMETER }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithInexistingAccount_MustReturnBadRequest()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithLoadAccount()
            .Build();
         var service = new AccountService(repository);

         var result = await service.DeleteAsync(Shared.EntityID.NewID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.ACCOUNT_NOT_FOUND }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithValidParameters_MustReturnOkResult()
      {
         var accountID = Shared.EntityID.NewID();
         var repository = AccountRepositoryMocker
            .Create()
            .WithLoadAccount(new AccountEntity(accountID, "Account Text", enAccountType.General, null, null, true))
            .Build();
         var service = new AccountService(repository);

         var result = await service.DeleteAsync(accountID);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
