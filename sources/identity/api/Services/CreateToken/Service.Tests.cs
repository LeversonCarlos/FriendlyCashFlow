using System;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      internal async void CreateTokenAsync_WithNullParameter_MustThrowException()
      {
         var service = new IdentityService(null, null);

         var value = await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateTokenAsync(null));

         Assert.NotNull(value);
         Assert.Contains("The User parameter is required for the CreateTokenAsync function on the IdentityService class", value.Message);
      }

   }
}
