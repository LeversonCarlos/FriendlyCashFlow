using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryRecurrenceEntityTests
   {

      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID recurrenceID, short currentOccurrence, short totalOccurrences)
      {
         var exception = Assert.Throws<ArgumentException>(() => EntryRecurrenceEntity.Restore(recurrenceID, currentOccurrence, totalOccurrences));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Restore_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_RECURRENCEID, null, null, null},
            new object[] { WARNINGS.INVALID_CURRENTOCCURRENCE, Shared.EntityID.MockerID(), null, null},
            new object[] { WARNINGS.INVALID_CURRENTOCCURRENCE, Shared.EntityID.MockerID(), 0, null},
            new object[] { WARNINGS.INVALID_CURRENTOCCURRENCE, Shared.EntityID.MockerID(), -1, null},
            new object[] { WARNINGS.INVALID_CURRENTOCCURRENCE, Shared.EntityID.MockerID(), EntryRecurrenceEntity.MaxOccurrence+1, null},
            new object[] { WARNINGS.INVALID_TOTALOCCURRENCES, Shared.EntityID.MockerID(), 1, null},
            new object[] { WARNINGS.INVALID_TOTALOCCURRENCES, Shared.EntityID.MockerID(), 1, 0},
            new object[] { WARNINGS.INVALID_TOTALOCCURRENCES, Shared.EntityID.MockerID(), 1, -1},
            new object[] { WARNINGS.INVALID_TOTALOCCURRENCES, Shared.EntityID.MockerID(), 1, EntryRecurrenceEntity.MaxOccurrence+1}
         };

      /*
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
      */

   }
}
