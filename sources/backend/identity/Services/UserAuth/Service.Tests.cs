using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void UserAuth_WithNullParameter_MustReturnBadResult()
      {
         var identityService = new IdentityService(null, null, null);

         var result = await identityService.UserAuthAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_USERAUTH_PARAMETER }, (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void UserAuth_WithInvalidUsername_MustReturnBadResult()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { } };
         var identityService = new IdentityService(identitySettings, null, null);

         var param = new UserAuthVM { UserName = "userName", Password = "password" };
         var result = await identityService.UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_USERNAME }, (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void UserAuth_WithInvalidPassword_MustReturnBadResult()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } };
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserName()
            .Build();
         var identityService = new IdentityService(identitySettings, userRepository, null);

         var param = new UserAuthVM { UserName = "userName@xpto.com", Password = "password" };
         var result = await identityService.UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.PASSWORD_MINIMUM_SIZE }, (result.Result as BadRequestObjectResult).Value);
      }

      [Theory]
      [MemberData(nameof(UserAuth_WithInvalidAuthData_MustReturnBadResult_Data))]
      public async void UserAuth_WithInvalidAuthData_MustReturnBadResult(IUserEntity results)
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { } };
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserName(results)
            .Build();
         var identityService = new IdentityService(identitySettings, userRepository, null);

         var param = new UserAuthVM { UserName = "userName@xpto.com", Password = "password" };
         var result = await identityService.UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED }, (result.Result as BadRequestObjectResult).Value);
      }
      public static IEnumerable<object[]> UserAuth_WithInvalidAuthData_MustReturnBadResult_Data() =>
         new[] {
            new object[] { (UserEntity)null },
            new object[] {  new UserEntity("userName@xpto.com", "not-hashed-password") }
         };

      [Fact]
      public async void UserAuth_WithInvalidSettingsThatThrowsException_MustReturnBadResult()
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserName(new UserEntity("userName@xpto.com", "X03MO1qnZdYdgyfeuILPmQ=="))
            .Build();
         var identitySettings = new IdentitySettings
         {
            PasswordRules = new PasswordRuleSettings { MinimumSize = 5 },
            Token = new TokenSettings { }
         };
         var identityService = new IdentityService(identitySettings, userRepository, null);

         var param = new UserAuthVM { UserName = "userName@xpto.com", Password = "password" };
         var result = await identityService.UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { "The AccessExpirationInSeconds property on the Settings parameter is required for the GetTokenDescriptor function on the Token class (Parameter 'settings')" }, (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void UserAuth_WithValidParameters_MustReturnOkResult()
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserName(new UserEntity("userName@xpto.com", "X03MO1qnZdYdgyfeuILPmQ=="))
            .Build();
         var tokenRepository = TokenRepositoryMocker
            .Create()
            .Build();
         var identitySettings = new IdentitySettings
         {
            PasswordRules = new PasswordRuleSettings { MinimumSize = 5 },
            Token = new TokenSettings { SecuritySecret = "security-secret-security-secret", AccessExpirationInSeconds = 1, RefreshExpirationInSeconds = 60 }
         };
         var identityService = new IdentityService(identitySettings, userRepository, tokenRepository);

         var param = new UserAuthVM { UserName = "userName@xpto.com", Password = "password" };
         var result = await identityService.UserAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<TokenVM>((result.Result as OkObjectResult).Value);
         var tokenVM = (TokenVM)((result.Result as OkObjectResult).Value);
         Assert.NotEmpty(tokenVM.UserID);
         Assert.NotEmpty(tokenVM.AccessToken);
         Assert.NotEmpty(tokenVM.RefreshToken);
      }

   }
}
