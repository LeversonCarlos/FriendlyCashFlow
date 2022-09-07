namespace Lewio.CashFlow.Users;

public struct LoggedInUser
{
   public string UserID { get; private set; }

   public static implicit operator string(LoggedInUser value) => value.UserID;
   // public static explicit operator LoggedInUser(string id) => LoggedInUser.Create(id);

   public static LoggedInUser Create(string userID) =>
      new LoggedInUser
      {
         UserID = userID
      };
}
