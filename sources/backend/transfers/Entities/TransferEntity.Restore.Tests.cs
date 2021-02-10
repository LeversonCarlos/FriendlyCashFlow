using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferEntityTests
   {

      [Fact]
      public void Restore_WithValidParameters_MustResultInstance()
      {
         var transferID = Shared.EntityID.NewID();
         var expenseAccountID = Shared.EntityID.NewID();
         var incomeAccountID = Shared.EntityID.NewID();
         var date = DateTime.Now.AddDays(1);
         var value = (decimal)23.45;

         var result = TransferEntity.Restore(transferID, expenseAccountID, incomeAccountID, date, value);

         Assert.NotNull(result);
         Assert.Equal(transferID, result.TransferID);
         Assert.Equal(expenseAccountID, result.ExpenseAccountID);
         Assert.Equal(incomeAccountID, result.IncomeAccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(value, result.Value);
      }

      [Theory]
      [MemberData(nameof(Restore_WithInvalidParameters_MustThrowException_Data))]
      public void Restore_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID transferID, Shared.EntityID expenseAccountID, Shared.EntityID incomeAccountID, DateTime date, decimal value)
      {
         var exception = Assert.Throws<ArgumentException>(() => TransferEntity.Restore(transferID, expenseAccountID, incomeAccountID, date, value));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Restore_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { WARNINGS.INVALID_TRANSFERID, null, null, null, null, null},
            new object[] { WARNINGS.INVALID_EXPENSEACCOUNTID, Shared.EntityID.NewID(), null, null, null, null},
            new object[] { WARNINGS.INVALID_INCOMEACCOUNTID, Shared.EntityID.NewID(), Shared.EntityID.NewID(), null, null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.NewID(), Shared.EntityID.NewID(), Shared.EntityID.NewID(), null, null},
            new object[] { WARNINGS.INVALID_DATE, Shared.EntityID.NewID(), Shared.EntityID.NewID(), Shared.EntityID.NewID(), DateTime.MinValue, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.NewID(), Shared.EntityID.NewID(), Shared.EntityID.NewID(), DateTime.UtcNow, null},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.NewID(), Shared.EntityID.NewID(), Shared.EntityID.NewID(), DateTime.UtcNow, 0},
            new object[] { WARNINGS.INVALID_VALUE, Shared.EntityID.NewID(), Shared.EntityID.NewID(), Shared.EntityID.NewID(), DateTime.UtcNow, -0.01}
         };

   }
}
