using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Balances.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddBalanceService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddSingleton(x => Shared.Tests.InsightsServiceMocker.Create().Build())
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddBalanceService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<IBalanceService>();

         Assert.NotNull(service);
      }

      [Fact]
      internal void AddBalanceService_InjectedRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddBalanceService(configs)
            .Services
            .BuildServiceProvider();

         var repository = services.GetService<IBalanceRepository>();

         Assert.NotNull(repository);
      }

   }
}
