using Lewio.CashFlow.Accounts;
using Lewio.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

partial class AccountsTests
{

   [Theory]
   [InlineData("ABC")]
   [InlineData("AB")]
   [InlineData("BC")]
   [InlineData("AC")]
   public async void LoadCommand_FoundRow_BasedOn_ProvidedParameter(string accountID)
   {
      var serviceProvider = SearchCommand_GetServiceProvider();

      var request =  LoadRequestModel.Create(accountID);

      var response = await serviceProvider
         .GetRequiredService<LoadCommand>()
         .HandleAsync(request);
      response.EnsureValidResponse();

      var result = response.Account!.AccountID;
      Assert.Equal(accountID, result);
   }

   [Fact]
   public async void LoadCommand_WithInvalidAccountID_ResultsError()
   {
      var serviceProvider = LoadCommand_GetServiceProvider();

      var request = LoadRequestModel.Create("");

      var response = await serviceProvider
         .GetRequiredService<LoadCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("Invalid accountID parameter", ex.Message);
   }

   [Fact]
   public async void LoadCommand_WithNotFoundRow_ResultsError()
   {
      var serviceProvider = LoadCommand_GetServiceProvider();

      var request = LoadRequestModel.Create("SomeID");

      var response = await serviceProvider
         .GetRequiredService<LoadCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("Account not found", ex.Message);
   }

   [Fact]
   public async void LoadCommand_WithRemovedRow_ResultsError()
   {
      var serviceProvider = LoadCommand_GetServiceProvider();

      var request = LoadRequestModel.Create("YY");

      var response = await serviceProvider
         .GetRequiredService<LoadCommand>()
         .HandleAsync(request);

      var ex = Assert.Throws<Exception>(() => response.EnsureValidResponse());
      Assert.Equal("Account not found", ex.Message);
   }

   private IServiceProvider LoadCommand_GetServiceProvider()
   {
      var serviceProvider = Mocks.Builder
         .ServiceProvider()
         .Build();

      var loggedInUser = serviceProvider
         .GetRequiredService<Users.LoggedInUser>();

      var ctx = serviceProvider
         .GetRequiredService<Common.DataContext>();

      ctx.Accounts.Add(new AccountEntity { RowStatus=1, UserID = loggedInUser, AccountID = "ABC", Text = "AAA BBB CCC" });
      ctx.Accounts.Add(new AccountEntity { RowStatus=1, UserID = loggedInUser, AccountID = "AB", Text = "AAA BBB" });
      ctx.Accounts.Add(new AccountEntity { RowStatus=1, UserID = loggedInUser, AccountID = "AC", Text = "AAA CCC" });
      ctx.Accounts.Add(new AccountEntity { RowStatus=1, UserID = loggedInUser, AccountID = "BC", Text = "BBB CCC" });
      ctx.Accounts.Add(new AccountEntity { RowStatus=0, UserID = loggedInUser, AccountID = "YY", Text = "YYY" });

      ctx.SaveChanges();

      return serviceProvider;
   }

}
