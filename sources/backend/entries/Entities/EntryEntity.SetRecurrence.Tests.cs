using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   public partial class EntryEntityTests
   {

      [Fact]
      public void SetRecurrence_WithValidDate_MustApplyProperty()
      {
         var entity = EntryEntity.Builder().Build();
         var recurrence = EntryRecurrenceEntity.Restore(Shared.EntityID.MockerID(), 1, 10);

         entity.SetRecurrence(recurrence);

         Assert.NotNull(entity.Recurrence);
         Assert.Equal(recurrence.RecurrenceID, entity.Recurrence.RecurrenceID);

      }

   }
}
