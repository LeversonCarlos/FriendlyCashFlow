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

      [Fact]
      public void Restore_WithValidParameters_MustResultInstance()
      {
         var recurrenceID = Shared.EntityID.MockerID();
         var currentOccurrence = (short)1;
         var totalOccurrences = (short)10;

         var result = EntryRecurrenceEntity.Restore(recurrenceID, currentOccurrence, totalOccurrences);

         Assert.NotNull(result);
         Assert.Equal(recurrenceID, result.RecurrenceID);
         Assert.Equal(currentOccurrence, result.CurrentOccurrence);
         Assert.Equal(totalOccurrences, result.TotalOccurrences);
      }

      [Fact]
      internal void Clone_HashCode_MustBeEqual()
      {
         var recurrenceID = Shared.EntityID.MockerID();
         var currentOccurrence = (short)1;
         var totalOccurrences = (short)10;

         var first = EntryRecurrenceEntity.Restore(recurrenceID, currentOccurrence, totalOccurrences);
         var second = first.GetCopy();

         Assert.Equal(first.GetHashCode(), second.GetHashCode());
      }


   }
}
