using Lewio.CashFlow.Accounts;
using Lewio.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

partial class AccountsTests
{

   [Theory]
   [InlineData("Z", "")]
   [InlineData("AAAA", "")]
   [InlineData("AAA BBB", "ABC|AB")]
   [InlineData("AAA", "ABC|AB|AC")]
   [InlineData("A", "ABC|AB|AC")]
   [InlineData("", "ABC|AB|AC|BC")]
   public async void SearchCommand_FoundRows_BasedOn_ProvidedSearchTerms(string searchTerms, string expectedResult)
   {
      var serviceProvider = SearchCommand_GetServiceProvider();

      var request = new SearchRequestModel
      {
         SearchTerms = searchTerms
      };

      var response = await serviceProvider
         .GetRequiredService<SearchCommand>()
         .HandleAsync(request);
      response.EnsureValidResponse();

      var accountIDs = response.Accounts
         ?.Select(x => (string)x.AccountID)
         ?.ToArray() ?? new string[] { };
      var result = string.Join("|", accountIDs);

      Assert.Equal(expectedResult, result);
   }

   private IServiceProvider SearchCommand_GetServiceProvider()
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
