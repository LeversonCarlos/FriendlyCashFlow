using System;

namespace FriendlyCashFlow.Identity
{

   public interface IUser
   {
      string UserID { get; }
   }

   internal class User : IUser
   {

      public User()
      {
         UserID = System.Guid.NewGuid().ToString();
      }

      public User(string userID)
      {
         UserID = userID;
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

   }

}