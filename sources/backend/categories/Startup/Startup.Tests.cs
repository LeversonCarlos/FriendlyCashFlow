using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Categories.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddCategoryService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddSingleton<Shared.IInsightsService>(x => Shared.Tests.InsightsServiceMocker.Create().Build())
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddCategoryService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<ICategoryService>();

         Assert.NotNull(service);
      }

      [Fact]
      internal void AddCategoryService_InjectedRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddCategoryService(configs)
            .Services
            .BuildServiceProvider();

         var repository = services.GetService<ICategoryRepository>();

         Assert.NotNull(repository);
      }

   }
}
