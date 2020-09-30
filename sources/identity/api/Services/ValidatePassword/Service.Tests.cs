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
            new object[] { "Password", ValidatePasswordInteractor.WARNING.PASSWORD_REQUIRE_UPPER_CASES, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumUpperCases = 2 } } },
            new object[] { "pASSWORD", ValidatePasswordInteractor.WARNING.PASSWORD_REQUIRE_LOWER_CASES, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumLowerCases = 2 } } },
            new object[] { "passw0rd", ValidatePasswordInteractor.WARNING.PASSWORD_REQUIRE_NUMBERS, new IdentitySettings { PasswordRules = new PasswordRuleSettings {   MinimumNumbers=2 } } },
            new object[] { "p@ssword", ValidatePasswordInteractor.WARNING.PASSWORD_REQUIRE_SYMBOLS, new IdentitySettings { PasswordRules = new PasswordRuleSettings {   MinimumSymbols=2 } } },
            new object[] { "password", ValidatePasswordInteractor.WARNING.PASSWORD_MINIMUM_SIZE, new IdentitySettings { PasswordRules = new PasswordRuleSettings { MinimumSize = 10 } } }
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
