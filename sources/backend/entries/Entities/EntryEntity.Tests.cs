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

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID entryID, Patterns.IPatternEntity pattern, Shared.EntityID accountID, DateTime dueDate, decimal entryValue)
      {
         var exception = Assert.Throws<ArgumentException>(() => new EntryEntity(entryID, pattern, accountID, dueDate, entryValue));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_ENTRYID, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_PATTERN, Shared.EntityID.NewID(), null, null, null, null},
            new object[] { WARNINGS.INVALID_ACCOUNTID, Shared.EntityID.NewID(), new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern"), null, null, null},
            new object[] { WARNINGS.INVALID_DUEDATE, Shared.EntityID.NewID(), new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern"), Shared.EntityID.NewID(), null, null},
            new object[] { WARNINGS.INVALID_DUEDATE, Shared.EntityID.NewID(), new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern"), Shared.EntityID.NewID(), DateTime.MinValue, null},
            new object[] { WARNINGS.INVALID_ENTRYVALUE, Shared.EntityID.NewID(), new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern"), Shared.EntityID.NewID(), DateTime.UtcNow, null},
            new object[] { WARNINGS.INVALID_ENTRYVALUE, Shared.EntityID.NewID(), new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern"), Shared.EntityID.NewID(), DateTime.UtcNow, 0},
            new object[] { WARNINGS.INVALID_ENTRYVALUE, Shared.EntityID.NewID(), new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern"), Shared.EntityID.NewID(), DateTime.UtcNow, -0.01}
         };

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
