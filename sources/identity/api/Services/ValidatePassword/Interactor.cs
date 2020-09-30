using FriendlyCashFlow.Shared;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{
   internal class ValidatePasswordInteractor : Interactor<IUser, IdentitySettings, string, string[]>
   {

      public ValidatePasswordInteractor(IMongoDatabase mongoDatabase, IdentitySettings settings) :
         base(mongoDatabase, settings, IdentityService.CollectionName)
      { }

      internal struct WARNING
      {
         internal const string PASSWORD_REQUIRE_UPPER_CASES = "WARNING_IDENTITY_PASSWORD_REQUIRE_UPPER_CASES";
         internal const string PASSWORD_REQUIRE_LOWER_CASES = "WARNING_IDENTITY_PASSWORD_REQUIRE_LOWER_CASES";
         internal const string PASSWORD_REQUIRE_NUMBERS = "WARNING_IDENTITY_PASSWORD_REQUIRE_NUMBERS";
         internal const string PASSWORD_REQUIRE_SYMBOLS = "WARNING_IDENTITY_PASSWORD_REQUIRE_SYMBOLS";
         internal const string PASSWORD_MINIMUM_SIZE = "WARNING_IDENTITY_USERS_PASSWORD_MINIMUM_SIZE";
      }

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
            msgs.Add(WARNING.PASSWORD_REQUIRE_UPPER_CASES);
         if (passwordStrength.Lower < Settings.PasswordRules.MinimumLowerCases)
            msgs.Add(WARNING.PASSWORD_REQUIRE_LOWER_CASES);
         if (passwordStrength.Number < Settings.PasswordRules.MinimumNumbers)
            msgs.Add(WARNING.PASSWORD_REQUIRE_NUMBERS);
         if (passwordStrength.Symbol < Settings.PasswordRules.MinimumSymbols)
            msgs.Add(WARNING.PASSWORD_REQUIRE_SYMBOLS);
         if (passwordStrength.Size < Settings.PasswordRules.MinimumSize)
            msgs.Add(WARNING.PASSWORD_MINIMUM_SIZE);

         await Task.CompletedTask;
         return msgs.ToArray();
      }

   }
}
