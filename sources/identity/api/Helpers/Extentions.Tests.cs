using FriendlyCashFlow.Identity.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public class StartupExtentionsTests
   {

      [Fact]
      internal void AddIdentityService_Instance_MustNotBeNull()
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

   }
}
