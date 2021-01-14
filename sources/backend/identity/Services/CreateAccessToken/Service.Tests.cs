using System;
using Xunit;

namespace Elesse.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Fact]
      internal async void CreateAccessToken_WithNullParameter_MustThrowException()
      {
         var service = IdentityService.Create();

         var value = await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAccessTokenAsync(null));

         Assert.NotNull(value);
         Assert.Contains("The User parameter is required for the CreateTokenAsync function on the IdentityService class", value.Message);
      }

   }
}
