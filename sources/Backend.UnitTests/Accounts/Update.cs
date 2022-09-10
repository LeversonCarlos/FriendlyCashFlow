using Lewio.CashFlow.Accounts;
using Lewio.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

partial class AccountsTests
{

   [Fact]
   public async void UpdateCommand_MustUpdateRow_And_RetrieveItBackFromDatabase()
   {
      var serviceProvider = SearchCommand_GetServiceProvider();

      var request = new UpdateRequestModel
      {
         Account = new AccountModel
         {
            AccountID = "AC",
            Text = "AA CC",
            Type = AccountTypeEnum.General,
         }
      };

      var response = await serviceProvider
         .GetRequiredService<UpdateCommand>()
         .HandleAsync(request);
      response.EnsureValidResponse();

      var result = response.Account;
      Assert.Equal("AA CC", result.Text);
   }

   [Fact]
   public async void UpdateCommand_WithAnotherAccountWithSameText_ResultsError()
   {
      var serviceProvider = UpdateCommand_GetServiceProvider();

      var request = new UpdateRequestModel
      {
         Account = new AccountModel
         {
            AccountID = "AC",
            Text = "AAA BBB",
            Type = AccountTypeEnum.General,
         }
      };

      var response = await serviceProvider
         .GetRequiredService<UpdateCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("An account with this Text property already exists", ex.Message);
   }

   [Fact]
   public async void UpdateCommand_WithNotFoundAccount_ResultsError()
   {
      var serviceProvider = UpdateCommand_GetServiceProvider();

      var request = new UpdateRequestModel
      {
         Account = new AccountModel
         {
            AccountID = "ZZ",
            Text = "ZZZ",
            Type = AccountTypeEnum.General,
         }
      };

      var response = await serviceProvider
         .GetRequiredService<UpdateCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("Account not found", ex.Message);
   }

   private IServiceProvider UpdateCommand_GetServiceProvider()
   {
      var serviceProvider = Mocks.Builder
         .ServiceProvider()
         .Build();

      var loggedInUser = serviceProvider
         .GetRequiredService<Users.LoggedInUser>();

      var ctx = serviceProvider
         .GetRequiredService<Common.DataContext>();

      ctx.Accounts.Add(new AccountEntity { UserID = loggedInUser, AccountID = "ABC", Text = "AAA BBB CCC" });
      ctx.Accounts.Add(new AccountEntity { UserID = loggedInUser, AccountID = "AB", Text = "AAA BBB" });
      ctx.Accounts.Add(new AccountEntity { UserID = loggedInUser, AccountID = "AC", Text = "AAA CCC" });
      ctx.Accounts.Add(new AccountEntity { UserID = loggedInUser, AccountID = "BC", Text = "BBB CCC" });

      ctx.SaveChanges();

      return serviceProvider;
   }

}
