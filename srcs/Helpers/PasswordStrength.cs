using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FriendlyCashFlow.Helpers
{

   internal class PasswordStrengthService
   {

      private readonly IOptions<AppSettings> _appSettings;
      public PasswordStrengthService([FromServices] IOptions<AppSettings> appSettings) { this._appSettings = appSettings; }

      public PasswordStrengthResult Analyze(string value)
      {
         var result = new PasswordStrengthResult();

         foreach (var item in value)
         {
            if (Char.IsUpper(item)) { result.UpperCases++; }
            if (Char.IsLower(item)) { result.LowerCases++; }
            if (Char.IsNumber(item)) { result.Numbers++; }
            if (Char.IsSymbol(item)) { result.Symbols++; }
            result.Size++;
         }

         var passwordSettings = this._appSettings.Value.Passwords;
         result.IsUpperCasesAttended = !passwordSettings.RequireUpperCases || result.UpperCases > 0;
         result.IsLowerCasesAttended = !passwordSettings.RequireLowerCases || result.LowerCases > 0;
         result.IsNumbersAttended = !passwordSettings.RequireNumbers || result.Numbers > 0;
         result.IsSymbolsAttended = !passwordSettings.RequireSymbols || result.Symbols > 0;
         result.IsSizeAttended = result.Size >= passwordSettings.MinimumSize;

         return result;
      }

   }

   internal class PasswordStrengthResult
   {

      public short UpperCases { get; set; }
      public bool IsUpperCasesAttended { get; set; }

      public short LowerCases { get; set; }
      public bool IsLowerCasesAttended { get; set; }

      public short Numbers { get; set; }
      public bool IsNumbersAttended { get; set; }

      public short Symbols { get; set; }
      public bool IsSymbolsAttended { get; set; }

      public short Size { get; set; }
      public bool IsSizeAttended { get; set; }

   }

}
