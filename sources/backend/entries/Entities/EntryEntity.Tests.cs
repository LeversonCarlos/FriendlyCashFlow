using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Entries.Tests
{
   public partial class EntryEntityTests
   {

      [Fact]
      public void ValidInstance_SettingInvalidProperties_MustThrowException()
      {
         var pattern = new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern");
         var entity = EntryEntity.Create(pattern, Shared.EntityID.NewID(), DateTime.UtcNow, (decimal)23.45);

         var exception = Assert.Throws<ArgumentException>(() => entity.PayDate = DateTime.MinValue);

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PAYDATE, exception.Message);
      }

   }
}
