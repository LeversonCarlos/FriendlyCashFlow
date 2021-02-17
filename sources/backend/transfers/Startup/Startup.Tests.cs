using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Transfers.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddTransferService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddSingleton(x => Shared.Tests.InsightsServiceMocker.Create().Build())
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddTransferService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<ITransferService>();

         Assert.NotNull(service);
      }

      [Fact]
      internal void AddTransferService_InjectedRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddTransferService(configs)
            .Services
            .BuildServiceProvider();

         var repository = services.GetService<ITransferRepository>();

         Assert.NotNull(repository);
      }

   }
}
