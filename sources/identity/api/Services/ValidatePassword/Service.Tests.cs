using System.Collections.Generic;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [MemberData(nameof(ValidatePassword_WithInvalidParameters_MustResultErrorMessage_Data))]
      internal async void ValidatePassword_WithInvalidParameters_MustResultErrorMessage(string password, string warningMessage, PasswordSettings settings)
      {
         var identityService = new IdentityService(null, settings);

         var result = await identityService.ValidatePasswordAsync(password);

         Assert.Equal(new string[] { warningMessage }, result);
      }
      public static IEnumerable<object[]> ValidatePassword_WithInvalidParameters_MustResultErrorMessage_Data() =>
         new[] {
            new object[] { "Password", IdentityService.USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING, new PasswordSettings { MinimumUpperCases=2 } },
            new object[] { "pASSWORD", IdentityService.USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING, new PasswordSettings { MinimumLowerCases=2 } },
            new object[] { "passw0rd", IdentityService.USERS_PASSWORD_REQUIRE_NUMBERS_WARNING, new PasswordSettings { MinimumNumbers=2 } },
            new object[] { "p@ssword", IdentityService.USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING, new PasswordSettings { MinimumSymbols=2 } },
            new object[] { "password", IdentityService.USERS_PASSWORD_MINIMUM_SIZE_WARNING, new PasswordSettings { MinimumSize=10 } }
         };

      [Fact]
      internal async void ValidatePassword_WithValidParameters_MustResultOkRequest()
      {
         var settings = new PasswordSettings
         {
            MinimumUpperCases = 2,
            MinimumLowerCases = 2,
            MinimumNumbers = 2,
            MinimumSymbols = 2,
            MinimumSize = 10
         };
         var identityService = new IdentityService(null, settings);

         var password = "p#sSw0rD$1";
         var result = await identityService.ValidatePasswordAsync(password);

         Assert.Equal(new string[] { }, result);
      }

   }
}
