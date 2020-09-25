using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [InlineData((string)null)]
      [InlineData("")]
      [InlineData("abc")]
      [InlineData("abc.xpto")]
      [InlineData("abc.xpto.com")]
      [InlineData("abc-com")]
      [InlineData("abc_com")]
      internal async void ValidateUsername_WithInvalidParameters_MustResultErrorMessage(string username)
      {
         var identityService = new IdentityService(null, null);
         var result = await identityService.ValidateUsernameAsync(username);

         Assert.Equal(new string[] { IdentityService.WARNING_IDENTITY_INVALID_USERNAME }, result);
      }

   }
}
