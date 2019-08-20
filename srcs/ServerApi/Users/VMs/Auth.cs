namespace FriendlyCashFlow.API.Users
{

   public class AuthVM
   {
      public string GrantType { get; set; }
      public string UserName { get; set; }
      public string Password { get; set; }
      public string RefreshToken { get; set; }
   }

}
