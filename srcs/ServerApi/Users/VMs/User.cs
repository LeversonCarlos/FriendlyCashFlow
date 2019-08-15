namespace FriendlyCashFlow.API.Users
{

   public class CreateVM
   {
      public string UserName { get; set; }
      public string Password { get; set; }
   }

   public class UserVM
   {

      public long UserID { get; set; }
      public string UserName { get; set; }
      public string Text { get; set; }

      internal static UserVM Convert(UserData value)
      {
         return new UserVM
         {
            UserID = value.UserID,
            UserName = value.UserName,
            Text = value.Text
         };
      }

   }

}
