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
         var mongoClient = MongoMocker.Create().Build();
         var provider = ProviderMocker.Create().WithIdentityService(new IdentityService(mongoClient)).Build().BuildServiceProvider();
         var service = (IIdentityService)provider.GetService<IIdentityService>();

         var expected = IdentityService.WARNING_IDENTITY_INVALID_REGISTER_PARAMETER;
         var result = await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterAsync(null));

         Assert.NotNull(result);
         Assert.Equal(expected, result.Message);
      }

   }
}
