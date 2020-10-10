using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      public async void TokenAuth_WithNullParameter_MustReturnBadResult()
      {
         var identityService = new IdentityService(null, null);
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();

         var result = await provider.GetService<IIdentityService>().TokenAuthAsync(null);

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
         var identityService = new IdentityService(null, null);
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();
         var param = new TokenAuthVM { RefreshToken = refreshToken };

         var result = await provider.GetService<IIdentityService>().TokenAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER }, (result.Result as BadRequestObjectResult).Value);
      }

      [Theory]
      [MemberData(nameof(TokenAuth_WithInvalidAuthData_MustReturnBadResult_Data))]
      public async void TokenAuth_WithInvalidAuthData_MustReturnBadResult(string exceptionMessage, IRefreshToken results)
      {
         var mongoCollection = MongoCollectionMocker<IRefreshToken>
            .Create()
            .WithFindAndDelete(results)
            .Build();
         var mongoDatabase = MongoDatabaseMocker.Create().WithCollection(mongoCollection, IdentityService.GetRefreshTokenCollectionName()).Build();
         var service = new IdentityService(mongoDatabase, null);
         var param = new TokenAuthVM { RefreshToken = "refresh-token" };

         var result = await service.TokenAuthAsync(param);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(new string[] { exceptionMessage }, (result.Result as BadRequestObjectResult).Value);
      }
      public static IEnumerable<object[]> TokenAuth_WithInvalidAuthData_MustReturnBadResult_Data() =>
         new[] {
            new object[] { WARNINGS.AUTHENTICATION_HAS_FAILED, (RefreshToken)null },
            new object[] { WARNINGS.AUTHENTICATION_HAS_FAILED, RefreshToken.Create(System.Guid.NewGuid().ToString(), DateTime.UtcNow.AddMilliseconds(-1) ) }
         };

   }
}
