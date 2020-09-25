using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FriendlyCashFlow.Identity.Tests
{
   partial class IdentityServiceTests
   {

      [Theory]
      [MemberData(nameof(ValidatePassword_WithInvalidParameters_MustResultBadRequest_Data))]
      internal async void ValidatePassword_WithInvalidParameters_MustResultBadRequest(string password, string warningMessage, PasswordSettings settings)
      {
         var identityService = new IdentityService(null, settings);
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();

         var result = await provider.GetService<IIdentityService>().ValidatePasswordAsync(password);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new string[] { warningMessage }, (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }
      public static IEnumerable<object[]> ValidatePassword_WithInvalidParameters_MustResultBadRequest_Data() =>
         new[] {
            new object[] { "Password", "USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING", new PasswordSettings { MinimumUpperCases=2 } },
            new object[] { "pASSWORD", "USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING", new PasswordSettings { MinimumLowerCases=2 } },
            new object[] { "passw0rd", "USERS_PASSWORD_REQUIRE_NUMBERS_WARNING", new PasswordSettings { MinimumNumbers=2 } },
            new object[] { "p@ssword", "USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING", new PasswordSettings { MinimumSymbols=2 } },
            new object[] { "password", "USERS_PASSWORD_MINIMUM_SIZE_WARNING", new PasswordSettings { MinimumSize=10 } }
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
         var provider = ProviderMocker.Create().WithIdentityService(identityService).Build().BuildServiceProvider();

         var password = "p#sSw0rD$1";
         var result = await provider.GetService<IIdentityService>().ValidatePasswordAsync(password);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
