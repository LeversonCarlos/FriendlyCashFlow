using System;

namespace Elesse.Accounts
{

   internal class AccountEntity : IAccountEntity
   {

      /*
      public UserEntity(string text, enAccountType type, short? closingDay, short? dueDay)
         : this(Guid.NewGuid().ToString(), userName, password)
      { }

      public UserEntity(string userID, string userName, string password)
      {
         UserID = userID;
         UserName = userName;
         Password = password;
      }
      */

      Shared.EntityID _AccountID;
      [MongoDB.Bson.Serialization.Attributes.BsonId]
      public Shared.EntityID AccountID
      {
         get => _AccountID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNING_INVALID_ACCOUNTID);
            _AccountID = value;
         }
      }
      internal const string WARNING_INVALID_ACCOUNTID = "WARNING_ACCOUNTS_INVALID_ACCOUNTID_PARAMETER";

      string _Text;
      public string Text
      {
         get => _Text;
         internal set
         {
            if (string.IsNullOrEmpty(value) || value.Length > 100)
               throw new ArgumentException(WARNING_INVALID_TEXT);
            _Text = value;
         }
      }
      internal const string WARNING_INVALID_TEXT = "WARNING_ACCOUNTS_INVALID_TEXT_PARAMETER";

      bool _Active;
      public bool Active
      {
         get => _Active;
         internal set
         {
            if (value == null)
               throw new ArgumentException(WARNING_INVALID_ACTIVE);
            _Password = value;
         }
      }
      internal const string WARNING_INVALID_ACTIVE = "WARNING_ACCOUNTS_INVALID_ACCOUNT_PARAMETER";

   }

}
