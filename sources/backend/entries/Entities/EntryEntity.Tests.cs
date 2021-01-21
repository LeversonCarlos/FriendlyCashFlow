using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   public class EntryEntityTests
   {

      [Fact]
      public void Constructor_WithValidParameters_MustResultValidInstance()
      {
         var pattern = new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern");
         var entity = new EntryEntity(pattern, Shared.EntityID.NewID(), DateTime.UtcNow, (decimal)23.45);

         Assert.NotNull(entity);
         Assert.NotNull(entity.EntryID);
         Assert.Equal(36, ((string)entity.EntryID).Length);
         Assert.Equal(pattern.PatternID, entity.Pattern.PatternID);
         Assert.False(entity.Paid);
         Assert.Null(entity.PayDate);
      }

   }
}
