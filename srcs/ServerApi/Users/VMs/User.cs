namespace FriendlyCashFlow.API.Users
{

   public class CreateVM
   {
      public string Description { get; set; }
      public string UserName { get; set; }
      public string Password { get; set; }
   }

   public class UserVM
   {

      public string UserID { get; set; }
      public string Text { get; set; }

      internal static UserVM Convert(UserData value)
      {
         return new UserVM
         {
            UserID = value.UserID,
            Text = value.Text
         };
      }

   }

}
