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

      public User(string userName, Password password)
         : this(System.Guid.NewGuid().ToString(), userName, password)
      { }

      public User(string userID, string userName, Password password)
      {
         UserID = userID;
         UserName = userName;
         Password = password;
      }

      string _UserID;
      public string UserID
      {
         get => _UserID;
         private set
         {
            if (string.IsNullOrEmpty(value) || value.Length != 36)
               throw new ArgumentException(WARNING_IDENTITY_INVALID_USERID_PARAMETER);
            _UserID = value;
         }
      }
      internal const string WARNING_IDENTITY_INVALID_USERID_PARAMETER = "WARNING_IDENTITY_INVALID_USERID_PARAMETER";

      string _UserName;
      public string UserName
      {
         get => _UserName;
         private set
         {
            if (string.IsNullOrEmpty(value) || value.Length < 8 || value.Length > 50)
               throw new ArgumentException(WARNING_IDENTITY_INVALID_USERNAME_PARAMETER);
            _UserName = value;
         }
      }
      internal const string WARNING_IDENTITY_INVALID_USERNAME_PARAMETER = "WARNING_IDENTITY_INVALID_USERNAME_PARAMETER";

      Password _Password;
      public Password Password
      {
         get => _Password;
         private set
         {
            _Password = value;
         }
      }
      internal const string WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER = "WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER";

   }

}