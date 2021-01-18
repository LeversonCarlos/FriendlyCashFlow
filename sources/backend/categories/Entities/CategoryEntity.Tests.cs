using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Categories.Tests
{
   public class CategoryEntityTests
   {


      [Theory]
      [InlineData("Expense Text", enCategoryType.Expense, null)]
      [InlineData("Income Text", enCategoryType.Income, null)]
      [InlineData("Sub Category Text", enCategoryType.Income, "10fb4762-e6b2-45e5-be0f-d81a9289a493")]
      public void Constructor_WithValidParameters_MustResultValidInstance(string text, enCategoryType type, string parentStringID)
      {
         Shared.EntityID parentID = null;
         if (!string.IsNullOrWhiteSpace(parentStringID))
            parentID = (Shared.EntityID)parentStringID;
         var entity = new CategoryEntity(text, type, parentID);

         Assert.NotNull(entity);
         Assert.NotNull(entity.CategoryID);
         Assert.Equal(36, ((string)entity.CategoryID).Length);
         Assert.Equal(text, entity.Text);
         Assert.Equal(type, entity.Type);
         if (!string.IsNullOrWhiteSpace(parentStringID))
            Assert.Equal(parentStringID, (string)entity.ParentID);
         else
            Assert.Null(entity.ParentID);
         Assert.True(entity.RowStatus);
      }

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID categoryID, string text, enCategoryType type, Shared.EntityID parentID)
      {
         var exception = Assert.Throws<ArgumentException>(() => new CategoryEntity(categoryID, text, type, parentID));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_CATEGORYID, null, "Category Text", enCategoryType.Income, null},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), (string)null, enCategoryType.Income, null},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), "", enCategoryType.Income, null},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), " ", enCategoryType.Income, null},
            new object[] { WARNINGS.INVALID_TEXT, Shared.EntityID.NewID(), new string('0', 101), enCategoryType.Income, null}
         };

   }
}
