namespace FriendlyCashFlow.Identity
{

   public class IdentitySettings
   {
      public string PasswordSalt { get; set; }
      public PasswordRuleSettings PasswordRules { get; set; }
   }

   public class PasswordRuleSettings
   {
      public int MinimumUpperCases { get; set; }
      public int MinimumLowerCases { get; set; }
      public int MinimumNumbers { get; set; }
      public int MinimumSymbols { get; set; }
      public int MinimumSize { get; set; }
   }
}
