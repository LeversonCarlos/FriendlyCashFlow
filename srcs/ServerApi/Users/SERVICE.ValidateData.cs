
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace FriendlyCashFlow.API.Users
{
   partial class UsersService
   {

      private string[] ValidatePassword(string value)
      {
         try
         {
            var msgs = new List<string>();
            var appSettings = this.GetService<IOptions<AppSettings>>().Value;

            var passwordStrength = new Helpers.PasswordStrength(value);

            if (!passwordStrength.IsSizeAttended(appSettings.Passwords.MinimumSize))
            { msgs.Add("USERS_PASSWORD_MINIMUM_SIZE_WARNING"); }

            if (!passwordStrength.IsUpperCasesAttended(appSettings.Passwords.RequireUpperCases))
            { msgs.Add("USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING"); }

            if (!passwordStrength.IsLowerCasesAttended(appSettings.Passwords.RequireLowerCases))
            { msgs.Add("USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING"); }

            if (!passwordStrength.IsNumbersAttended(appSettings.Passwords.RequireNumbers))
            { msgs.Add("USERS_PASSWORD_REQUIRE_NUMBERS_WARNING"); }

            if (!passwordStrength.IsSymbolsAttended(appSettings.Passwords.RequireSymbols))
            { msgs.Add("USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING"); }

            return msgs.ToArray();

         }
         catch (Exception) { throw; }
      }

   }
}
