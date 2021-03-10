using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Patterns.Tests
{
   public class PatternEntityTests
   {

      [Theory]
      [InlineData(enPatternType.Expense, "10fb4762-e6b2-45e5-be0f-d81a9289a493", "Expense Text")]
      [InlineData(enPatternType.Income, "10fb4762-e6b2-45e5-be0f-d81a9289a493", "Income Text")]
      public void Constructor_WithValidParameters_MustResultValidInstance(enPatternType type, string categoryStringID, string text)
      {
         Shared.EntityID categoryID = (Shared.EntityID)categoryStringID;
         var entity = PatternEntity.Create(type, categoryID, text);

         Assert.NotNull(entity);
         Assert.NotNull(entity.PatternID);
         Assert.Equal(36, ((string)entity.PatternID).Length);
         Assert.Equal(type, entity.Type);
         Assert.Equal(text, entity.Text);
         Assert.Equal(categoryStringID, (string)entity.CategoryID);
         Assert.Equal(0, entity.RowsCount);
      }

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID patternID, enPatternType type, Shared.EntityID categoryID, string text)
      {
         var exception = Assert.Throws<ArgumentException>(() => PatternEntity.Restore(patternID, type, categoryID, text));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_PATTERNID, null, enPatternType.Income, null, null},
            new object[] { WARNINGS.INVALID_CATEGORYID, Shared.EntityID.NewID(), enPatternType.Income, null, null},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), enPatternType.Income, Shared.EntityID.NewID(), (string)null},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), enPatternType.Income, Shared.EntityID.NewID(), ""},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), enPatternType.Income, Shared.EntityID.NewID(), " "},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), enPatternType.Income, Shared.EntityID.NewID(), new string('0', 101)}
         };

      [Fact]
      public void TwoInstances_WithSameProperties_MustHaveSameHash()
      {
         var patternID = Shared.EntityID.NewID();
         var categoryID = Shared.EntityID.NewID();
         var text = "My Pattern Description";
         var first = PatternEntity.Restore(patternID, enPatternType.Income, categoryID, text);
         var second = PatternEntity.Restore(patternID, enPatternType.Income, categoryID, text);

         var result = first == second;

         Assert.True(result);
      }

   }
}
