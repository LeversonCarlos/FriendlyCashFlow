using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void Register_WithNullParameter_MustReturnBadResult()
      {
         var identityService = IdentityService.Create();

         var result = await identityService.RegisterAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_REGISTER_PARAMETER }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithInvalidUsername_MustReturnBadResult()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } };
         var identityService = IdentityService.Create(identitySettings);

         var registerParam = new RegisterVM { UserName = "userName", Password = "password" };
         var result = await identityService.RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_USERNAME }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithInvalidPassword_MustReturnBadResult()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } };
         var identityService = IdentityService.Create(identitySettings);

         var registerParam = new RegisterVM { UserName = "userName@xpto.com", Password = "password" };
         var result = await identityService.RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.PASSWORD_MINIMUM_SIZE }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithExistingUserName_MustReturnBadRequest()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { } };
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserName(null, new UserEntity("userName@xpto.com", "password"))
            .Build();
         var identityService = new IdentityService(identitySettings, userRepository, null);
         var registerParam = new RegisterVM { UserName = "userName@xpto.com", Password = "password" };

         var result = await identityService.RegisterAsync(registerParam);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);

         result = await identityService.RegisterAsync(registerParam);
         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.USERNAME_ALREADY_USED }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Register_WithValidParameters_MustReturnOkResult()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 5 } };
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserName()
            .Build();
         var identityService = new IdentityService(identitySettings, userRepository, null);

         var registerParam = new RegisterVM { UserName = "userName@xpto.com", Password = "password" };
         var result = await identityService.RegisterAsync(registerParam);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
