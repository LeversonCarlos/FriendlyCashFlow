using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Principal;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [MemberData(nameof(ChangePassword_WithNullParameters_MustReturnBadResult_Data))]
      public async void ChangePassword_WithNullParameters_MustReturnBadResult(IIdentity identity, ChangePasswordVM changePasswordVM)
      {
         var identityService = new IdentityService(null, null, null);

         var result = await identityService.ChangePasswordAsync(identity, changePasswordVM);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER }, (result as BadRequestObjectResult).Value);
      }
      public static IEnumerable<object[]> ChangePassword_WithNullParameters_MustReturnBadResult_Data() =>
         new[] {
            new object[] { (IIdentity)null, (ChangePasswordVM)null },
            new object[] { (IIdentity)(new Mock<IIdentity>().Object), (ChangePasswordVM)null },
            new object[] { (IIdentity)(new GenericIdentity("my-user-id" )), (ChangePasswordVM)null }
         };

      [Theory]
      [InlineData(" ", "")]
      [InlineData("password", "")]
      [InlineData("lengthy-password", " ")]
      [InlineData("lengthy-password", "password")]
      public async void ChangePassword_WithInvalidPassword_MustReturnBadResult(string oldPassword, string newPassword)
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } };
         var identityService = new IdentityService(identitySettings, null, null);

         var param = new ChangePasswordVM { OldPassword = oldPassword, NewPassword = newPassword };
         var result = await identityService.ChangePasswordAsync(new GenericIdentity("my-user-id"), param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.PASSWORD_MINIMUM_SIZE }, (result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void ChangePassword_WithSamePassword_MustReturnBadResult()
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } };
         var identityService = new IdentityService(identitySettings, null, null);

         var param = new ChangePasswordVM { OldPassword = "same-password", NewPassword = "same-password" };
         var result = await identityService.ChangePasswordAsync(new GenericIdentity("my-user-id"), param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.INVALID_CHANGEPASSWORD_PARAMETER }, (result as BadRequestObjectResult).Value);
      }

      [Theory]
      [MemberData(nameof(ChangePassword_WithNotFoundUser_MustReturnBadResult_Data))]
      public async void ChangePassword_WithNotFoundUser_MustReturnBadResult(IUserEntity[] results)
      {
         var identitySettings = new IdentitySettings { PasswordRules = new PasswordRuleSettings { } };
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserID(results)
            .Build();
         var identityService = new IdentityService(identitySettings, userRepository, null);

         var param = new ChangePasswordVM { OldPassword = "password", NewPassword = "new-password" };
         var result = await identityService.ChangePasswordAsync(new GenericIdentity("my-user-id"), param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED }, (result as BadRequestObjectResult).Value);
      }
      public static IEnumerable<object[]> ChangePassword_WithNotFoundUser_MustReturnBadResult_Data() =>
         new[] {
            new object[] { new IUserEntity[] { } },
            new object[] { new IUserEntity[] { new UserEntity("userName@xpto.com", "not-hashed-password") } }
         };

      [Fact]
      public async void ChangePassword_WithSuccessExecution_MustReturnOkResult()
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserID(new UserEntity("userName@xpto.com", "X03MO1qnZdYdgyfeuILPmQ=="))
            .Build();
         var identitySettings = new IdentitySettings
         {
            PasswordRules = new PasswordRuleSettings { MinimumSize = 5 },
            Token = new TokenSettings { SecuritySecret = "security-secret-security-secret", AccessExpirationInSeconds = 1, RefreshExpirationInSeconds = 60 }
         };
         var identityService = new IdentityService(identitySettings, userRepository, null);

         var param = new ChangePasswordVM { OldPassword = "password", NewPassword = "new-password" };
         var result = await identityService.ChangePasswordAsync(new GenericIdentity("my-user-id"), param);

         Assert.NotNull(result);
         Assert.IsType<OkResult>(result);
      }

   }
}
