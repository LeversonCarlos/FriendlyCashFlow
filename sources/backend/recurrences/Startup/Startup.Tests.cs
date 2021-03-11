using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddRecurrenceService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddSingleton(x => Shared.Tests.InsightsServiceMocker.Create().Build())
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddRecurrenceService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<IRecurrenceService>();

         Assert.NotNull(service);
      }

      [Fact]
      internal void AddRecurrenceService_InjectedRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddRecurrenceService(configs)
            .Services
            .BuildServiceProvider();

         var repository = services.GetService<IRecurrenceRepository>();

         Assert.NotNull(repository);
      }

   }
}
