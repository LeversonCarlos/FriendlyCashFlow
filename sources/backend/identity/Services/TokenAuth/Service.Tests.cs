using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void TokenAuth_WithNullParameter_MustReturnBadResult()
      {
         var identityService = IdentityService.Create();

         var result = await identityService.TokenAuthAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER }, (result.Result as BadRequestObjectResult).Value);
      }

      [Theory]
      [InlineData((string)null)]
      [InlineData("")]
      [InlineData(" ")]
      public async void TokenAuth_WithEmptyRefreshToken_MustReturnBadResult(string refreshToken)
      {
         var identityService = IdentityService.Create();

         var param = new TokenAuthVM { RefreshToken = refreshToken };
         var result = await identityService.TokenAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER }, (result.Result as BadRequestObjectResult).Value);
      }

      [Theory]
      [MemberData(nameof(TokenAuth_WithInvalidRefreshTokenData_MustReturnBadResult_Data))]
      public async void TokenAuth_WithInvalidRefreshTokenData_MustReturnBadResult(string exceptionMessage, ITokenEntity results)
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .Build();
         var tokenRepositoty = TokenRepositoryMocker
            .Create()
            .WithRetrieveRefreshToken(results)
            .Build();
         var service = IdentityService.Create(userRepository, tokenRepositoty);

         var param = new TokenAuthVM { RefreshToken = "refresh-token" };
         var result = await service.TokenAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { exceptionMessage }, (result.Result as BadRequestObjectResult).Value);
      }
      public static IEnumerable<object[]> TokenAuth_WithInvalidRefreshTokenData_MustReturnBadResult_Data() =>
         new[] {
            new object[] { WARNINGS.AUTHENTICATION_HAS_FAILED, (TokenEntity)null },
            new object[] { WARNINGS.AUTHENTICATION_HAS_FAILED, TokenEntity.Create(Guid.NewGuid().ToString(), DateTime.UtcNow.AddMilliseconds(-1) ) }
         };

      [Fact]
      public async void TokenAuth_WithNotFoundUser_MustReturnBadResult()
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserID()
            .Build();
         var tokenRepository = TokenRepositoryMocker
            .Create()
            .WithRetrieveRefreshToken(TokenEntity.Create(System.Guid.NewGuid().ToString(), DateTime.UtcNow.AddMinutes(1)))
            .Build();
         var identityService = IdentityService.Create(userRepository, tokenRepository);

         var param = new TokenAuthVM { RefreshToken = "refresh-token" };
         var result = await identityService.TokenAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED }, (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void TokenAuth_WithInvalidSettings_MustReturnBadResult()
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserID(new UserEntity("userName@xpto.com", "X03MO1qnZdYdgyfeuILPmQ=="))
            .Build();
         var tokenRepository = TokenRepositoryMocker
            .Create()
            .WithRetrieveRefreshToken(TokenEntity.Create(System.Guid.NewGuid().ToString(), DateTime.UtcNow.AddMinutes(1)))
            .Build();
         var identitySettings = new IdentitySettings
         {
            PasswordRules = new PasswordRuleSettings { MinimumSize = 5 },
            Token = new TokenSettings { }
         };
         var identityService = IdentityService.Create(identitySettings, userRepository, tokenRepository);

         var param = new TokenAuthVM { RefreshToken = "refresh-token" };
         var result = await identityService.TokenAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { "The AccessExpirationInSeconds property on the Settings parameter is required for the GetTokenDescriptor function on the Token class (Parameter 'settings')" }, (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void TokenAuth_WithValidParameters_MustReturnOkResult()
      {
         var userRepository = UserRepositoryMocker
            .Create()
            .WithGetUserByUserID(new UserEntity("userName@xpto.com", "X03MO1qnZdYdgyfeuILPmQ=="))
            .Build();
         var tokenRepository = TokenRepositoryMocker
            .Create()
            .WithRetrieveRefreshToken(TokenEntity.Create(System.Guid.NewGuid().ToString(), DateTime.UtcNow.AddMinutes(1)))
            .Build();
         var identitySettings = new IdentitySettings
         {
            PasswordRules = new PasswordRuleSettings { MinimumSize = 5 },
            Token = new TokenSettings { SecuritySecret = "security-secret-security-secret", AccessExpirationInSeconds = 1, RefreshExpirationInSeconds = 60 }
         };
         var identityService = IdentityService.Create(identitySettings, userRepository, tokenRepository);

         var param = new TokenAuthVM { RefreshToken = "refresh-token" };
         var result = await identityService.TokenAuthAsync(param);

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
