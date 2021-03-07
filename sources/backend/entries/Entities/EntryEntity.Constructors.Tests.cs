using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryEntityTests
   {

      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID entryID, Patterns.IPatternEntity pattern, Shared.EntityID accountID, DateTime dueDate, decimal value, bool paid, DateTime? payDate)
      {
         var exception = Assert.Throws<ArgumentException>(() => EntryEntity.Restore(entryID, pattern, accountID, dueDate, value, paid, payDate));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Restore_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_ENTRYID, null, null, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_PATTERN, Shared.EntityID.NewID(), null, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_ACCOUNTID, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), null, null, null, null, null},
            new object[] { WARNINGS.INVALID_DUEDATE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), null, null, null, null},
            new object[] { WARNINGS.INVALID_DUEDATE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.MinValue, null, null, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, null, null, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, 0, null, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, -0.01, null, null },
            new object[] { WARNINGS.INVALID_PAYDATE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, 1, true, DateTime.MinValue }
         };

      [Fact]
      public void Restore_WithValidParameters_MustResultInstance()
      {
         var entryID = Shared.EntityID.NewID();
         var pattern = Patterns.PatternEntity.Builder().Build();
         var accountID = Shared.EntityID.NewID();
         var dueDate = DateTime.Now.AddDays(1);
         var value = (decimal)23.45;
         var paid = true;
         var payDate = DateTime.Now.AddDays(1);

         var result = EntryEntity.Restore(entryID, pattern, accountID, dueDate, value, paid, payDate);

         Assert.NotNull(result);
         Assert.Equal(entryID, result.EntryID);
         Assert.Equal(pattern, result.Pattern);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(dueDate, result.DueDate);
         Assert.Equal(value, result.Value);
         Assert.Equal(paid, result.Paid);
         Assert.Equal(payDate, result.PayDate);
      }

      [Fact]
      public void Create_WithValidParameters_MustResultInstance()
      {
         var pattern = Patterns.PatternEntity.Builder().Build();
         var accountID = Shared.EntityID.MockerID();
         var dueDate = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);

         var result = EntryEntity.Create(pattern, accountID, dueDate, value);

         Assert.NotNull(result);
         Assert.NotNull(result.EntryID);
         Assert.Equal(36, ((string)result.EntryID).Length);
         Assert.Equal(pattern, result.Pattern);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(dueDate, result.DueDate);
         Assert.Equal(value, result.Value);
      }

   }
}
