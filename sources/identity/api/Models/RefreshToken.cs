using System;

namespace FriendlyCashFlow.Identity
{

   public interface IRefreshToken
   {
      string TokenID { get; }
      string UserID { get; }
      DateTime ExpirationDate { get; }
   }

   internal class RefreshToken : IRefreshToken
   {

      RefreshToken() { }
      internal partial struct WARNINGS { }

      public static RefreshToken Create(string userID, DateTime expirationDate)
      {
         return new RefreshToken
         {
            TokenID = System.Guid.NewGuid().ToString(),
            UserID = userID,
            ExpirationDate = expirationDate
         };
      }

      string _TokenID;
      [MongoDB.Bson.Serialization.Attributes.BsonId]
      public string TokenID
      {
         get => _TokenID;
         private set
         {
            _TokenID = value;
         }
      }

      string _UserID;
      public string UserID
      {
         get => _UserID;
         private set
         {
            if (string.IsNullOrEmpty(value) || value.Length != 36)
               throw new ArgumentException(WARNINGS.INVALID_USERID_PARAMETER);
            _UserID = value;
         }
      }
      partial struct WARNINGS { public const string INVALID_USERID_PARAMETER = "WARNING_IDENTITY_INVALID_REFRESHTOKEN_USERID_PARAMETER"; }

      DateTime _ExpirationDate;
      public DateTime ExpirationDate
      {
         get => _ExpirationDate;
         private set
         {
            if (value == null || value <= DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_EXPIRATIONDATE_PARAMETER);
            _ExpirationDate = value;
         }
      }
      partial struct WARNINGS { public const string INVALID_EXPIRATIONDATE_PARAMETER = "WARNING_IDENTITY_INVALID_REFRESHTOKEN_EXPIRATIONDATE_PARAMETER"; }

   }

}