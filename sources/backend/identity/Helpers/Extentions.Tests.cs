using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Elesse.Identity.Helpers.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddIdentityService_IdentityService_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddControllers()
            .AddIdentityService(configs)
            .Services
            .BuildServiceProvider();

         var identityService = services.GetService<IIdentityService>();

         Assert.NotNull(identityService);
      }

      [Fact]
      internal void AddIdentityService_UserRepository_MustNotBeNull()
      {
         var configs = new ConfigurationBuilder()
            .Build();
         var services = new ServiceCollection()
            .AddSingleton<IMongoDatabase>(x => null)
            .AddControllers()
            .AddIdentityService(configs)
            .Services
            .BuildServiceProvider();

         var userRepository = services.GetService<IUserRepository>();

         Assert.NotNull(userRepository);
      }

   }
}
