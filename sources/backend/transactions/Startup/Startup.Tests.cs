using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Transactions.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddTransactionService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddSingleton(x => Shared.Tests.InsightsServiceMocker.Create().Build())
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddTransactionService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<ITransactionService>();

         Assert.NotNull(service);
      }

   }
}
