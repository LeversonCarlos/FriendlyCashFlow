using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void Register_WithInvalidParameters_MustReturnBadResult()
      {
         var identityService = new IdentityService(null, null);
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();

         var result = await provider.GetService<IIdentityService>().RegisterAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { RegisterInteractor.WARNING.INVALID_REGISTER_PARAMETER }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithInvalidPassword_MustReturnBadResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var registerParam = new RegisterVM { UserName = "userName@xpto.com", Password = "password" };

         var result = await provider.GetService<IIdentityService>().RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { Interactors.ValidatePassword.USERS_PASSWORD_MINIMUM_SIZE_WARNING }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithInvalidUsername_MustReturnBadResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var registerParam = new RegisterVM { UserName = "userName", Password = "password" };

         var result = await provider.GetService<IIdentityService>().RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { ValidateUsernameInteractor.WARNING.INVALID_USERNAME }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithValidParameters_MustReturnOkResult()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 5 } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var registerParam = new RegisterVM { UserName = "userName@xpto.com", Password = "password" };

         var result = await provider.GetService<IIdentityService>().RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

      [Fact]
      public async void Register_WithSameUserName_MustReturnBadRequest()
      {
         var mongoCollection = MongoCollectionMocker<IUser>.Create().WithCount(0, 1).Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection).Build();
         var identityService = new IdentityService(mongoDatabase, new IdentitySettings { PasswordRules = new PasswordRuleSettings { } });
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var registerParam = new RegisterVM { UserName = "userName@xpto.com", Password = "password" };

         var result = await provider.GetService<IIdentityService>().RegisterAsync(registerParam);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);

         result = await provider.GetService<IIdentityService>().RegisterAsync(registerParam);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { RegisterInteractor.WARNING.USERNAME_ALREADY_USED }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

   }
}
