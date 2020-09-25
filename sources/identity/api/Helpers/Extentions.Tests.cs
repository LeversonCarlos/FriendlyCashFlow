using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;

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
            .AddIdentityService(configs)
            .BuildServiceProvider();

         var value = services.GetService<IIdentityService>();

         Assert.NotNull(value);
      }

   }
}
