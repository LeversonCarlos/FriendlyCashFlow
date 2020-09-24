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
         var identityService = new IdentityService(null, null);
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();

         var expected = IdentityService.WARNING_IDENTITY_INVALID_REGISTER_PARAMETER;
         var result = await Assert.ThrowsAsync<ArgumentException>(() => provider.GetService<IIdentityService>().RegisterAsync(null));

         Assert.NotNull(result);
         Assert.Equal(expected, result.Message);
      }

      [Fact]
      public async void Register_WithValidParameters_MustReturnOkResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new ValidatePasswordSettings { MinimumSize = 5 });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var registerParam = new RegisterVM { UserName = "userName", Password = "password" };

         var result = await provider.GetService<IIdentityService>().RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
