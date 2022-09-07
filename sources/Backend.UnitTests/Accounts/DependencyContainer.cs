using Lewio.CashFlow.Accounts;
using Microsoft.Extensions.DependencyInjection;

namespace Lewio.CashFlow.UnitTests;

partial class AccountsTests
{

   [Fact]
   public void AccountCommands_WasInjected_IntoTheDependencyContainer()
   {
      var serviceProvider = Mocks.Builder
         .ServiceProvider()
         .Build();

      var searchCommand = serviceProvider
         .GetService<SearchCommand>();

      Assert.NotNull(searchCommand);
   }

}
