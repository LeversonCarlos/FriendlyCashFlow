using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Accounts
{
   internal class AccountEntity : IAccountEntity
   {

      public AccountEntity(string text, enAccountType type, short? closingDay, short? dueDay)
         : this(Shared.EntityID.NewID(), text, type, closingDay, dueDay, true)
      {
         RowStatus = true;
      }

      public AccountEntity(Shared.EntityID accountID, string text, enAccountType type, short? closingDay, short? dueDay, bool active)
      {
         AccountID = accountID;
         Text = text;
         Type = type;
         ClosingDay = closingDay;
         DueDay = dueDay;
         Active = active;
      }

      Shared.EntityID _AccountID;
      [BsonId]
      public Shared.EntityID AccountID
      {
         get => _AccountID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_ACCOUNTID);
            _AccountID = value;
         }
      }

      string _Text;
      public string Text
      {
         get => _Text;
         internal set
         {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
               throw new ArgumentException(WARNINGS.INVALID_TEXT);
            _Text = value;
         }
      }

      enAccountType _Type;
      public enAccountType Type
      {
         get => _Type;
         internal set
         {
            _Type = value;
         }
      }

      short? _ClosingDay;
      public short? ClosingDay
      {
         get => _ClosingDay;
         internal set
         {
            if (value.HasValue && (value.Value < 1 || value.Value > 31))
               throw new ArgumentException(WARNINGS.INVALID_CLOSING_DAY);
            _ClosingDay = value;
         }
      }

      short? _DueDay;
      public short? DueDay
      {
         get => _DueDay;
         internal set
         {
            if (value.HasValue && (value.Value < 1 || value.Value > 31))
               throw new ArgumentException(WARNINGS.INVALID_DUE_DAY);
            _DueDay = value;
         }
      }

      bool _Active;
      public bool Active
      {
         get => _Active;
         internal set
         {
            _Active = value;
         }
      }

      public bool RowStatus { get; set; }

   }
}
