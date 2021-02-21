using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   public partial class EntryEntityTests
   {

      [Fact]
      public void ClearPayment_MustSetPaidToFalse_AndClearPayDate()
      {
         var entity = EntryEntity.Builder().WithPayDate(Shared.Faker.GetFaker().Date.Soon()).Build();

         entity.ClearPayment();

         Assert.False(entity.Paid);
         Assert.Null(entity.PayDate);
      }

      [Fact]
      public void SetPayment_WithInvalidDate_MustThrowException()
      {
         var entity = EntryEntity.Builder().Build();

         var exception = Assert.Throws<ArgumentException>(() => entity.SetPayment(DateTime.MinValue, entity.Value));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PAYDATE, exception.Message);

      }

   }
}
