using System;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      internal async void CreateRefreshTokenAsync_WithNullParameter_MustThrowException()
      {
         var service = new IdentityService(null, null);

         var value = await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateRefreshTokenAsync(null));

         Assert.NotNull(value);
         Assert.Contains("The User parameter is required for the CreateTokenAsync function on the IdentityService class", value.Message);
      }

      [Fact]
      internal async void CreateRefreshTokenAsync_WithInvalidSettings_MustThrowException()
      {
         var settings = new IdentitySettings { Token = new TokenSettings { } };
         var user = new User("user@Name.com", "password");
         var service = new IdentityService(null, settings);

         var value = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateRefreshTokenAsync(user));

         Assert.NotNull(value);
         Assert.Equal("The RefreshExpirationInSeconds property on token settings is required for the CreateRefreshTokenAsync function on the IdentityService class", value.Message);
      }

   }
}
