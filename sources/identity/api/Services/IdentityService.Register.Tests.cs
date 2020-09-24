using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void Register_WithInvalidParameters_MustThrowException()
      {
         var provider = ProviderMocker.Create().WithIdentityService(new IdentityService(null)).Build().BuildServiceProvider();
         var service = (IIdentityService)provider.GetService<IIdentityService>();

         var expected = IdentityService.WARNING_IDENTITY_INVALID_REGISTER_PARAMETER;
         var result = await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterAsync(null));

         Assert.NotNull(result);
         Assert.Equal(expected, result.Message);
      }

      [Fact]
      public async void Register_WithValidParameters_MustReturnOkResult()
      {
         var mongoDatabase = MongoConnector.Create().BuildDatabase();
         var provider = ProviderMocker.Create().WithIdentityService(new IdentityService(mongoDatabase)).Build().BuildServiceProvider();
         var service = (IIdentityService)provider.GetService<IIdentityService>();
         var registerParam = new RegisterVM { UserName = "userName", Password = "password" };

         var result = await service.RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
