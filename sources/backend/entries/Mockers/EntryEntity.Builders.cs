using System;

namespace Elesse.Entries
{
   partial class EntryEntity
   {
      public static Tests.EntryEntityBuilder Builder() => new Tests.EntryEntityBuilder();
   }
}
namespace Elesse.Entries.Tests
{
   internal class EntryEntityBuilder
   {

      Shared.EntityID _EntryID = Shared.EntityID.MockerID();
      public EntryEntityBuilder WithEntryID(Shared.EntityID entryID)
      {
         _EntryID = entryID;
         return this;
      }

      Patterns.IPatternEntity _Pattern = Patterns.PatternEntity.Builder().Build();
      public EntryEntityBuilder WithPattern(Patterns.IPatternEntity pattern)
      {
         _Pattern = pattern;
         return this;
      }

      Shared.EntityID _AccountID = Shared.EntityID.MockerID();
      public EntryEntityBuilder WithAccountID(Shared.EntityID accountID)
      {
         _AccountID = accountID;
         return this;
      }

      DateTime _DueDate = Shared.Faker.GetFaker().Date.Soon();
      public EntryEntityBuilder WithDueDate(DateTime dueDate)
      {
         _DueDate = dueDate;
         return this;
      }

      decimal _EntryValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
      public EntryEntityBuilder WithEntryValue(decimal entryValue)
      {
         _EntryValue = entryValue;
         return this;
      }

      bool _Paid = false;
      DateTime? _PayDate = null;
      public EntryEntityBuilder WithPayDate(DateTime payDate)
      {
         _Paid = true;
         _PayDate = payDate;
         return this;
      }

      public EntryEntity Build() =>
         EntryEntity.Restore(_EntryID, _Pattern, _AccountID, _DueDate, _EntryValue, _Paid, _PayDate);

   }
}
