using Lewio.CashFlow.Accounts;
using Lewio.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

partial class AccountsTests
{

   [Fact]
   public async void CreateCommand_MustInsertRow_And_RetrieveItBackFromDatabase()
   {
      var serviceProvider = SearchCommand_GetServiceProvider();

      var request = new CreateRequestModel
      {
         Account = new AccountModel
         {
            Text = "CCC BBB AAA",
            Type = AccountTypeEnum.General,
         }
      };

      var response = await serviceProvider
         .GetRequiredService<CreateCommand>()
         .HandleAsync(request);
      response.EnsureValidResponse();

      var result = response.Account;
      Assert.Equal("CCC BBB AAA", result.Text);
   }

   [Fact]
   public async void CreateCommand_WithAnotherAccountWithSameText_ResultsError()
   {
      var serviceProvider = CreateCommand_GetServiceProvider();

      var request = new CreateRequestModel
      {
         Account = new AccountModel
         {
            Text = "AAA BBB",
            Type = AccountTypeEnum.General,
         }
      };

      var response = await serviceProvider
         .GetRequiredService<CreateCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("An account with this Text property already exists", ex.Message);
   }

   private IServiceProvider CreateCommand_GetServiceProvider()
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
