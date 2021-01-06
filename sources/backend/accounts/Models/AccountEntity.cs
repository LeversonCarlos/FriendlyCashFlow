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

      enAccountType _Type;
      public enAccountType Type
      {
         get => _Type;
         internal set
         {
            if (value == null)
               throw new ArgumentException(WARNING_INVALID_TYPE);
            _Text = value;
         }
      }
      internal const string WARNING_INVALID_TYPE = "WARNING_ACCOUNTS_INVALID_TYPE_PARAMETER";

      short? _ClosingDay;
      public short? ClosingDay
      {
         get => _ClosingDay;
         internal set
         {
            if (value.HasValue && (value.Value < 1 || value.Value > 31))
               throw new ArgumentException(WARNING_INVALID_CLOSING_DAY);
            _ClosingDay = value;
         }
      }
      internal const string WARNING_INVALID_CLOSING_DAY = "WARNING_ACCOUNTS_INVALID_CLOSING_DAY_PARAMETER";

      short? _DueDay;
      public short? DueDay
      {
         get => _DueDay;
         internal set
         {
            if (value.HasValue && (value.Value < 1 || value.Value > 31))
               throw new ArgumentException(WARNING_INVALID_DUE_DAY);
            _DueDay = value;
         }
      }
      internal const string WARNING_INVALID_DUE_DAY = "WARNING_ACCOUNTS_INVALID_DUE_DAY_PARAMETER";

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
