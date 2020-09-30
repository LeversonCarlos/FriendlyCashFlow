using FriendlyCashFlow.Shared;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity.Interactors
{
   internal class ValidatePassword : Interactor<IUser, IdentitySettings, string, string[]>
   {

      public ValidatePassword(IMongoDatabase mongoDatabase, IdentitySettings settings) :
         base(mongoDatabase, settings, IdentityService.CollectionName)
      { }

      internal const string USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING = "USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING";
      internal const string USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING = "USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING";
      internal const string USERS_PASSWORD_REQUIRE_NUMBERS_WARNING = "USERS_PASSWORD_REQUIRE_NUMBERS_WARNING";
      internal const string USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING = "USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING";
      internal const string USERS_PASSWORD_MINIMUM_SIZE_WARNING = "USERS_PASSWORD_MINIMUM_SIZE_WARNING";

      public override async Task<string[]> ExecuteAsync(string password)
      {

         var passwordChars = password
            .ToCharArray()
            .Select(item => new
            {
               Char = item,
               IsUpper = Char.IsUpper(item),
               IsLower = Char.IsLower(item),
               IsNumber = Char.IsNumber(item),
               IsSymbol = Char.IsSymbol(item) || !Char.IsLetterOrDigit(item)
            })
            .ToArray();

         var passwordStrength = passwordChars
            .Select(x => new
            {
               Upper = x.IsUpper ? 1 : 0,
               Lower = x.IsLower ? 1 : 0,
               Number = x.IsNumber ? 1 : 0,
               Symbol = x.IsSymbol ? 1 : 0,
               Size = 1
            })
            .GroupBy(x => new { })
            .Select(x => new
            {
               Upper = x.Sum(s => s.Upper),
               Lower = x.Sum(s => s.Lower),
               Number = x.Sum(s => s.Number),
               Symbol = x.Sum(s => s.Symbol),
               Size = x.Sum(s => s.Size)
            })
            .FirstOrDefault();

         var msgs = new List<string>();
         if (passwordStrength.Upper < Settings.PasswordRules.MinimumUpperCases)
            msgs.Add(USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING);
         if (passwordStrength.Lower < Settings.PasswordRules.MinimumLowerCases)
            msgs.Add(USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING);
         if (passwordStrength.Number < Settings.PasswordRules.MinimumNumbers)
            msgs.Add(USERS_PASSWORD_REQUIRE_NUMBERS_WARNING);
         if (passwordStrength.Symbol < Settings.PasswordRules.MinimumSymbols)
            msgs.Add(USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING);
         if (passwordStrength.Size < Settings.PasswordRules.MinimumSize)
            msgs.Add(USERS_PASSWORD_MINIMUM_SIZE_WARNING);

         await Task.CompletedTask;
         return msgs.ToArray();
      }

   }
}
