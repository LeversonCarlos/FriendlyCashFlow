using Lewio.CashFlow.Accounts;
using Lewio.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

partial class AccountsTests
{

   [Fact]
   public async void DeleteCommand_MustRemoveRow_And_NotFindItOnFollowingSearch()
   {
      var serviceProvider = SearchCommand_GetServiceProvider();
      var accountID = "AC";

      var loadRequest = LoadRequestModel.Create(accountID);
      var loadResponse = await serviceProvider
         .GetRequiredService<LoadCommand>()
         .HandleAsync(loadRequest);
      loadResponse.EnsureValidResponse();

      var result = loadResponse.Account!.AccountID;
      Assert.Equal(accountID, result);

      var deleteRequest = DeleteRequestModel.CreateFrom(accountID);
      var deleteResponse = await serviceProvider
         .GetRequiredService<DeleteCommand>()
         .HandleAsync(deleteRequest);
      deleteResponse.EnsureValidResponse();

      loadRequest = LoadRequestModel.Create(accountID);
      loadResponse = await serviceProvider
         .GetRequiredService<LoadCommand>()
         .HandleAsync(loadRequest);

      var ex = Assert.Throws<Exception>(() => loadResponse.EnsureValidResponse());
      Assert.Equal("Account not found", ex.Message);

   }

   [Fact]
   public async void DeleteCommand_WithNotFoundRow_ResultsError()
   {
      var serviceProvider = DeleteCommand_GetServiceProvider();
      var accountID = "ZZ";

      var request = DeleteRequestModel.CreateFrom(accountID);

      var response = await serviceProvider
         .GetRequiredService<DeleteCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("Account not found", ex.Message);
   }

   private IServiceProvider DeleteCommand_GetServiceProvider()
   {
      var serviceProvider = Mocks.Builder
         .ServiceProvider()
         .Build();

      var loggedInUser = serviceProvider
         .GetRequiredService<Users.LoggedInUser>();

      var ctx = serviceProvider
         .GetRequiredService<Common.DataContext>();

      ctx.Accounts.Add(new AccountEntity { RowStatus = 1, UserID = loggedInUser, AccountID = "ABC", Text = "AAA BBB CCC" });
      ctx.Accounts.Add(new AccountEntity { RowStatus = 1, UserID = loggedInUser, AccountID = "AB", Text = "AAA BBB" });
      ctx.Accounts.Add(new AccountEntity { RowStatus = 1, UserID = loggedInUser, AccountID = "AC", Text = "AAA CCC" });
      ctx.Accounts.Add(new AccountEntity { RowStatus = 1, UserID = loggedInUser, AccountID = "BC", Text = "BBB CCC" });

      ctx.SaveChanges();

      return serviceProvider;
   }

}
