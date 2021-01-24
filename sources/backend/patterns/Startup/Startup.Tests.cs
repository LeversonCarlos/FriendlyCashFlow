using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Patterns.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddPatternService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddSingleton(x => Shared.Tests.InsightsServiceMocker.Create().Build())
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddPatternService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<IPatternService>();

         Assert.NotNull(service);
      }

      [Fact]
      internal void AddEntryService_InjectedRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddPatternService(configs)
            .Services
            .BuildServiceProvider();

         var repository = services.GetService<IPatternRepository>();

         Assert.NotNull(repository);
      }

   }
}