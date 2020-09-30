using System.Collections.Generic;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [MemberData(nameof(ValidatePassword_WithInvalidParameters_MustResultErrorMessage_Data))]
      internal async void ValidatePassword_WithInvalidParameters_MustResultErrorMessage(string password, string warningMessage, IdentitySettings settings)
      {
         var identityService = new IdentityService(null, settings);

         var result = await identityService.ValidatePasswordAsync(password);

         Assert.Equal(new string[] { warningMessage }, result);
      }
      public static IEnumerable<object[]> ValidatePassword_WithInvalidParameters_MustResultErrorMessage_Data() =>
         new[] {
            new object[] { "Password", Interactors.ValidatePassword.USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumUpperCases = 2 } } },
            new object[] { "pASSWORD", Interactors.ValidatePassword.USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumLowerCases = 2 } } },
            new object[] { "passw0rd", Interactors.ValidatePassword.USERS_PASSWORD_REQUIRE_NUMBERS_WARNING, new IdentitySettings { PasswordRules = new PasswordRuleSettings {   MinimumNumbers=2 } } },
            new object[] { "p@ssword", Interactors.ValidatePassword.USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING, new IdentitySettings { PasswordRules = new PasswordRuleSettings {   MinimumSymbols=2 } } },
            new object[] { "password", Interactors.ValidatePassword.USERS_PASSWORD_MINIMUM_SIZE_WARNING, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } } }
         };

      [Fact]
      internal async void ValidatePassword_WithValidParameters_MustResultOkRequest()
      {
         var settings = new IdentitySettings
         {
            PasswordRules = new PasswordRuleSettings
            {
               MinimumUpperCases = 2,
               MinimumLowerCases = 2,
               MinimumNumbers = 2,
               MinimumSymbols = 2,
               MinimumSize = 10
            }
         };
         var identityService = new IdentityService(null, settings);

         var password = "p#sSw0rD$1";
         var result = await identityService.ValidatePasswordAsync(password);

         Assert.Equal(new string[] { }, result);
      }

   }
}
