using System;

namespace FriendlyCashFlow.Helpers
{
   internal class PasswordStrength
   {

      public PasswordStrength(string value)
      {
         foreach (var item in value)
         {
            if (Char.IsUpper(item)) { this.UpperCases++; }
            if (Char.IsLower(item)) { this.LowerCases++; }
            if (Char.IsNumber(item)) { this.Numbers++; }
            if (Char.IsSymbol(item)) { this.Symbols++; }
            this.Size++;
         }
      }

      public short UpperCases { get; }
      public bool IsUpperCasesAttended(bool isRequired) { return isRequired || this.UpperCases > 0; }

      public short LowerCases { get; }
      public bool IsLowerCasesAttended(bool isRequired) { return isRequired || this.LowerCases > 0; }

      public short Numbers { get; }
      public bool IsNumbersAttended(bool isRequired) { return isRequired || this.Numbers > 0; }

      public short Symbols { get; }
      public bool IsSymbolsAttended(bool isRequired) { return isRequired || this.Symbols > 0; }

      public short Size { get; }
      public bool IsSizeAttended(short min) { return this.Size >= min; }

   }
}
