using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Elesse.Accounts.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddAccountService_InjectedService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddAccountService(configs)
            .Services
            .BuildServiceProvider();

         var service = services.GetService<IAccountService>();

         Assert.NotNull(service);
      }

      [Fact]
      internal void AddIdentityService_UserRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddScoped(x => new Mock<Identity.IUser>().Object)
            .AddControllers()
            .AddAccountService(configs)
            .Services
            .BuildServiceProvider();

         var repository = services.GetService<IAccountRepository>();

         Assert.NotNull(repository);
      }

   }
}
