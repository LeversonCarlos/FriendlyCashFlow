using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void UserAuth_WithNullParameter_MustReturnBadResult()
      {
         var identityService = new IdentityService(null, null);
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();

         var result = await provider.GetService<IIdentityService>().UserAuthAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { UserAuthInteractor.WARNING.INVALID_USERAUTH_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void UserAuth_WithInvalidUsername_MustReturnBadResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var param = new UserAuthVM { UserName = "userName", Password = "password" };

         var result = await provider.GetService<IIdentityService>().UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { ValidateUsernameInteractor.WARNING.INVALID_USERNAME }, (result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void UserAuth_WithInvalidPassword_MustReturnBadResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var param = new UserAuthVM { UserName = "userName@xpto.com", Password = "password" };

         var result = await provider.GetService<IIdentityService>().UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { ValidatePasswordInteractor.WARNING.PASSWORD_MINIMUM_SIZE }, (result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void UserAuth_WithValidParameters_MustReturnOkResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 5 } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var param = new UserAuthVM { UserName = "userName@xpto.com", Password = "password" };

         var result = await provider.GetService<IIdentityService>().UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkResult>(result);
      }

   }
}
