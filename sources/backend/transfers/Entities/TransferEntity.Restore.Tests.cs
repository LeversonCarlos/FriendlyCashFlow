using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferEntityTests
   {

      [Fact]
      public void Restore_WithValidParameters_MustResultInstance()
      {
         var transferID = Shared.EntityID.NewID();
         var expenseAccountID = Shared.EntityID.NewID();
         var incomeAccountID = Shared.EntityID.NewID();
         var date = DateTime.Now.AddDays(1);
         var value = (decimal)23.45;

         var result = TransferEntity.Restore(transferID, expenseAccountID, incomeAccountID, date, value);

         Assert.NotNull(result);
         Assert.Equal(transferID, result.TransferID);
         Assert.Equal(expenseAccountID, result.ExpenseAccountID);
         Assert.Equal(incomeAccountID, result.IncomeAccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(value, result.Value);
      }

      /*
      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID entryID, Patterns.IPatternEntity pattern, Shared.EntityID accountID, DateTime dueDate, decimal entryValue, bool paid, DateTime? payDate)
      {
         var exception = Assert.Throws<ArgumentException>(() => EntryEntity.Restore(entryID, pattern, accountID, dueDate, entryValue, paid, payDate));

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
            new object[] { WARNINGS.INVALID_ENTRYVALUE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, null, null, null},
            new object[] { WARNINGS.INVALID_ENTRYVALUE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, 0, null, null},
            new object[] { WARNINGS.INVALID_ENTRYVALUE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, -0.01, null, null },
            new object[] { WARNINGS.INVALID_PAYDATE, Shared.EntityID.NewID(), Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.UtcNow, 1, true, DateTime.MinValue }
         };
      */

   }
}
