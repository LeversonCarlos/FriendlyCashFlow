using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Elesse.Entries
{

   internal partial class EntryEntity : IEntryEntity
   {

      internal EntryEntity()
      {
         this.SearchDate = DateTime.MinValue;
         this.Sorting = 0;
      }

      public EntryEntity(Patterns.IPatternEntity pattern, Shared.EntityID accountID,
         DateTime dueDate, decimal entryValue)
         : this(Shared.EntityID.NewID(), pattern, accountID, dueDate, entryValue)
      { }

      public EntryEntity(Shared.EntityID entryID, Patterns.IPatternEntity pattern, Shared.EntityID accountID,
         DateTime dueDate, decimal entryValue)
      {
         EntryID = entryID;
         Pattern = pattern;
         AccountID = accountID;
         DueDate = dueDate;
         EntryValue = entryValue;
      }

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
         internal set
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
         internal set
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
         internal set
         {
            if (value == null || value == DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_DUEDATE);
            _DueDate = value;
         }
      }

      decimal _EntryValue;
      public decimal EntryValue
      {
         get => _EntryValue;
         internal set
         {
            if (value <= 0)
               throw new ArgumentException(WARNINGS.INVALID_ENTRYVALUE);
            _EntryValue = value;
         }
      }

      bool _Paid;
      public bool Paid
      {
         get => _Paid;
         internal set
         {
            _Paid = value;
         }
      }

      DateTime? _PayDate;
      public DateTime? PayDate
      {
         get => _PayDate;
         internal set
         {
            if (value == DateTime.MinValue)
               throw new ArgumentException(WARNINGS.INVALID_PAYDATE);
            _PayDate = value;
         }
      }

      internal DateTime SearchDate { get; set; }
      internal decimal Sorting { get; set; }

   }

   partial struct WARNINGS
   {
      internal const string INVALID_ENTRYID = "INVALID_ENTRYID_PARAMETER";
      internal const string INVALID_PATTERN = "INVALID_PATTERN_PARAMETER";
      internal const string INVALID_ACCOUNTID = "INVALID_ACCOUNTID_PARAMETER";
      internal const string INVALID_DUEDATE = "INVALID_DUEDATE_PARAMETER";
      internal const string INVALID_ENTRYVALUE = "INVALID_ENTRYVALUE_PARAMETER";
      internal const string INVALID_PAYDATE = "INVALID_PAYDATE_PARAMETER";
   }

}
