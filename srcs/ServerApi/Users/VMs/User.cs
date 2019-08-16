namespace FriendlyCashFlow.API.Users
{

   public class CreateVM
   {
      public string UserText { get; set; }
      public string UserMail { get; set; }
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
