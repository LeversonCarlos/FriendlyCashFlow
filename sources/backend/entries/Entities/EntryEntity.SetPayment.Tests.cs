using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   public partial class EntryEntityTests
   {

      [Fact]
      public void ClearPayment_MustSetPaidToFalse_AndClearPayDate()
      {
         var pattern = new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern");
         var entity = EntryEntity.Create(pattern, Shared.EntityID.NewID(), DateTime.UtcNow, (decimal)23.45);
         entity.SetPayment(DateTime.Now.AddDays(2), entity.EntryValue);

         entity.ClearPayment();

         Assert.False(entity.Paid);
         Assert.Null(entity.PayDate);
      }

      [Fact]
      public void SetPayment_WithInvalidDate_MustThrowException()
      {
         var pattern = new Patterns.PatternEntity(Patterns.enPatternType.Income, Shared.EntityID.NewID(), "My Pattern");
         var entity = EntryEntity.Create(pattern, Shared.EntityID.NewID(), DateTime.UtcNow, (decimal)23.45);

         var exception = Assert.Throws<ArgumentException>(() => entity.SetPayment(DateTime.MinValue, entity.EntryValue));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PAYDATE, exception.Message);

      }

   }
}
