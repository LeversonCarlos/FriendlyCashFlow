using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow.Identity.Tests
{
   public class IdentityServiceTests
   {

      [Fact]
      public async void Register_WithInvalidParameters_MustThrowException()
      {
         var mongoDatabase = MongoConnector.Create().BuildDatabase();
         var provider = ProviderMocker.Create().WithIdentityService(new IdentityService(mongoDatabase)).Build().BuildServiceProvider();
         var service = (IIdentityService)provider.GetService<IIdentityService>();

         var expected = IdentityService.WARNING_IDENTITY_INVALID_REGISTER_PARAMETER;
         var result = await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterAsync(null));

         Assert.NotNull(result);
         Assert.Equal(expected, result.Message);
      }

      /*
      [Fact]
      public async void Register_WithValidParameters_MustThrowException()
      {
         var mongoClient = MongoMocker.Create().Build();
         var provider = ProviderMocker.Create().WithIdentityService(new IdentityService(mongoClient)).Build().BuildServiceProvider();
         var service = (IIdentityService)provider.GetService<IIdentityService>();

         var param = new RegisterVM { UserName = "UserName", Password = "Password" };
         // var expected = new RegisterVM();
         var result = await service.RegisterAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }
      */

   }
}
