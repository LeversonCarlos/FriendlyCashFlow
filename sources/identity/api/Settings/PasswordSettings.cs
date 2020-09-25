namespace FriendlyCashFlow.Identity
{
   internal class PasswordSettings
   {
      public string PasswordSalt { get; set; }
      public int MinimumUpperCases { get; set; }
      public int MinimumLowerCases { get; set; }
      public int MinimumNumbers { get; set; }
      public int MinimumSymbols { get; set; }
      public int MinimumSize { get; set; }
   }
}
