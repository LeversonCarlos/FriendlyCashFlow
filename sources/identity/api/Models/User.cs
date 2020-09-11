using System;

namespace FriendlyCashFlow.Identity
{

   public interface IUser
   {
      string UserID { get; }
      string UserName { get; }
   }

   internal class User : IUser
   {

      public User(string userName)
         : this(System.Guid.NewGuid().ToString(), userName)
      { }

      public User(string userID, string userName)
      {
         UserID = userID;
         UserName = userName;
      }

      string _UserID;
      public string UserID
      {
         get => _UserID;
         private set
         {
            if (string.IsNullOrEmpty(value) || value.Length != 36)
               throw new ArgumentException("WARNING_IDENTITY_INVALID_USERID_DATA");
            _UserID = value;
         }
      }

      string _UserName;
      public string UserName
      {
         get => _UserName;
         private set
         {
            if (string.IsNullOrEmpty(value) || value.Length < 8 || value.Length > 50)
               throw new ArgumentException("WARNING_IDENTITY_INVALID_USERNAME_DATA");
            _UserName = value;
         }
      }

   }

}