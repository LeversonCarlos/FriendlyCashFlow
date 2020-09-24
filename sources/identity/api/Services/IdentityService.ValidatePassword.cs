using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public async Task<ActionResult> ValidatePasswordAsync(string password)
      {

         var passwordChars = password
            .ToCharArray()
            .Select(item => new
            {
               Char = item,
               IsUpper = Char.IsUpper(item),
               IsLower = Char.IsLower(item),
               IsNumber = Char.IsNumber(item),
               IsSymbol = Char.IsSymbol(item)
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
         if (passwordStrength.Upper < _Settings.MinimumUpperCases)
            msgs.Add("USERS_PASSWORD_REQUIRE_UPPER_CASES_WARNING");
         if (passwordStrength.Lower < _Settings.MinimumLowerCases)
            msgs.Add("USERS_PASSWORD_REQUIRE_LOWER_CASES_WARNING");
         if (passwordStrength.Number < _Settings.MinimumNumbers)
            msgs.Add("USERS_PASSWORD_REQUIRE_NUMBERS_WARNING");
         if (passwordStrength.Symbol < _Settings.MinimumSymbols)
            msgs.Add("USERS_PASSWORD_REQUIRE_SYMBOLS_WARNING");
         if (passwordStrength.Size < _Settings.MinimumSize)
            msgs.Add("USERS_PASSWORD_MINIMUM_SIZE_WARNING");
         if (msgs.Any())
            return new BadRequestObjectResult(msgs.ToArray());

         await Task.CompletedTask;
         return new OkResult();
      }

   }

   partial interface IIdentityService
   {
      Task<ActionResult> ValidatePasswordAsync(string password);
   }

   internal class ValidatePasswordSettings
   {
      // public string PasswordSalt { get; set; }
      public int MinimumUpperCases { get; set; }
      public int MinimumLowerCases { get; set; }
      public int MinimumNumbers { get; set; }
      public int MinimumSymbols { get; set; }
      public int MinimumSize { get; set; }
   }

}
