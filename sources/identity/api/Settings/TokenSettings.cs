namespace FriendlyCashFlow.Identity
{
   public class TokenSettings
   {
      public string SecuritySecret { get; set; }
      public string Issuer { get; set; }
      public string Audience { get; set; }
      public int AccessExpirationInSeconds { get; set; }
   }
}
