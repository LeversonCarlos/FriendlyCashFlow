namespace Elesse.Identity
{
   public class IdentitySettings
   {
      public string PasswordSalt { get; set; }
      public PasswordRuleSettings PasswordRules { get; set; }
      public TokenSettings Token { get; set; }
   }
}
