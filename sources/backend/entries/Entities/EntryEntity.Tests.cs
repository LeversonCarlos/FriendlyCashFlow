using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Entries.Tests
{
   public partial class EntryEntityTests
   {

      [Fact]
      public void Constructor_WithValidParameters_MustResultValidInstance()
      {
         var accountID = Shared.EntityID.NewID();
         var dueDate = DateTime.UtcNow;
         var entryValue = (decimal)23.45;
         var pattern = new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern");
         var entity = new EntryEntity(pattern, accountID, dueDate, entryValue);

         Assert.NotNull(entity);
         Assert.NotNull(entity.EntryID);
         Assert.Equal(36, ((string)entity.EntryID).Length);
         Assert.Equal(pattern.PatternID, entity.Pattern.PatternID);
         Assert.Equal(accountID, entity.AccountID);
         Assert.Equal(dueDate, entity.DueDate);
         Assert.Equal(entryValue, entity.EntryValue);
         Assert.False(entity.Paid);
         Assert.Null(entity.PayDate);
      }

      [Fact]
      public void ValidInstance_SettingInvalidProperties_MustThrowException()
      {
         var pattern = new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern");
         var entity = new EntryEntity(pattern, Shared.EntityID.NewID(), DateTime.UtcNow, (decimal)23.45);

         var exception = Assert.Throws<ArgumentException>(() => entity.PayDate = DateTime.MinValue);

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PAYDATE, exception.Message);
      }

   }
}
