using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryEntityTests
   {

      [Fact]
      public void SetSorting_WithValidParameters_MustReflectChanges()
      {
         var entity = EntryEntity.Create(Patterns.PatternEntity.Mock(), Shared.EntityID.NewID(), DateTime.Now.AddDays(10), (decimal)54.32);

         entity.SetSorting();

         Assert.Equal(entity.DueDate, entity.SearchDate);
         Assert.True(entity.Sorting > 0);
      }

   }
}
