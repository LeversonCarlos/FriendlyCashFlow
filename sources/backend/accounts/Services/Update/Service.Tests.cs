using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Fact]
      public async void Update_WithNullParameter_MustReturnBadResult()
      {
         var service = new AccountService(null);

         var result = await service.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      /*
      [Fact]
      public async void Insert_WithInvalidType_MustReturnBadResult()
      {
         var service = new AccountService(null);

         var param = new InsertVM { Text = "Account Text", Type = enAccountType.Bank, ClosingDay = 1, DueDay = 1 };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithExistingText_MustReturnBadRequest()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts(null, new AccountEntity[] { new AccountEntity("Account Text", enAccountType.General, null, null) })
            .Build();
         var service = new AccountService(repository);
         var param = new InsertVM { Text = "Account Text", Type = enAccountType.General };

         var result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);

         result = await service.InsertAsync(param);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.ACCOUNT_TEXT_ALREADY_USED }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithValidParameters_MustReturnOkResult()
      {
         var repository = AccountRepositoryMocker
            .Create()
            .WithSearchAccounts()
            .Build();
         var service = new AccountService(repository);

         var param = new InsertVM { Text = "Account Text", Type = enAccountType.General };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }
      */

   }
}
