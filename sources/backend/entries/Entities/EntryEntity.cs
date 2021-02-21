using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Entries
{

   internal partial class EntryEntity : IEntryEntity
   {

      Shared.EntityID _EntryID;
      [BsonId]
      public Shared.EntityID EntryID
      {
         get => _EntryID;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_ENTRYID);
            _EntryID = value;
         }
      }

      Patterns.IPatternEntity _Pattern;
      public Patterns.IPatternEntity Pattern
      {
         get => _Pattern;
         private set
         {
            if (value == null)
               throw new ArgumentException(WARNINGS.INVALID_PATTERN);
            _Pattern = value;
         }
      }

      Shared.EntityID _AccountID;
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

      DateTime _DueDate;
      public DateTime DueDate
      {
         get => _DueDate;
         private set
         {
            if (value == null || value == DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_DUEDATE);
            _DueDate = value;
         }
      }

      decimal _Value;
      public decimal Value
      {
         get => _Value;
         private set
         {
            if (value <= 0)
               throw new ArgumentException(WARNINGS.INVALID_VALUE);
            _Value = value;
         }
      }

      bool _Paid;
      public bool Paid
      {
         get => _Paid;
         private set
         {
            _Paid = value;
         }
      }

      DateTime? _PayDate;
      public DateTime? PayDate
      {
         get => _PayDate;
         private set
         {
            if (value == DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_PAYDATE);
            _PayDate = value;
         }
      }

      public DateTime SearchDate { get; set; }
      public decimal Sorting { get; set; }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_ENTRYID = "INVALID_ENTRYID_PARAMETER";
      internal const string INVALID_PATTERN = "INVALID_PATTERN_PARAMETER";
      internal const string INVALID_ACCOUNTID = "INVALID_ACCOUNTID_PARAMETER";
      internal const string INVALID_DUEDATE = "INVALID_DUEDATE_PARAMETER";
      internal const string INVALID_VALUE = "INVALID_VALUE_PARAMETER";
      internal const string INVALID_PAYDATE = "INVALID_PAYDATE_PARAMETER";
   }

}
