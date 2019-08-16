using System;
using System.Collections.Generic;

namespace FriendlyCashFlow.API.Users
{
   partial class UsersService
   {

      private string[] ValidatePassword(string value)
      {
         try
         {
            var msgs = new List<string>();
            var passwordStrength = this.GetService<Helpers.PasswordStrengthService>().Analyze(value);

            if (!passwordStrength.IsSizeAttended)
            { msgs.Add(string.Format(this.GetTranslation("USERS_PASSWORD_MINIMUM_SIZE_WARNING"), passwordStrength.MinimumSize)); }

            if (!passwordStrength.IsUpperCasesAttended)
            { msgs.Add(this.GetTranslation("USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING")); }

            if (!passwordStrength.IsLowerCasesAttended)
            { msgs.Add(this.GetTranslation("USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING")); }

            if (!passwordStrength.IsNumbersAttended)
            { msgs.Add("USERS_PASSWORD_REQUIRE_NUMBERS_WARNING"); }

            if (!passwordStrength.IsSymbolsAttended)
            { msgs.Add("USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING"); }

            return msgs.ToArray();

         }
         catch (Exception) { throw; }
      }

   }
}
