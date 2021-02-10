using System;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferEntityTests
   {

      [Fact]
      public void Create_WithValidParameters_MustResultInstance()
      {
         var expenseAccountID = Shared.EntityID.MockerID();
         var incomeAccountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);

         var result = TransferEntity.Create(expenseAccountID, incomeAccountID, date, value);

         Assert.NotNull(result);
         Assert.NotNull(result.TransferID);
         Assert.Equal(36, ((string)result.TransferID).Length);
         Assert.Equal(expenseAccountID, result.ExpenseAccountID);
         Assert.Equal(incomeAccountID, result.IncomeAccountID);
         Assert.Equal(date, result.Date);
         Assert.Equal(value, result.Value);
      }

      [Fact]
      public void Create_WithEqualExpenseAndIncomeAccounts_MustThrowException()
      {
         var expenseAccountID = Shared.EntityID.MockerID();
         var incomeAccountID = expenseAccountID;
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);

         var exception = Assert.Throws<ArgumentException>(() => TransferEntity.Create(expenseAccountID, incomeAccountID, date, value));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_EQUAL_EXPENSE_AND_INCOME_ACCOUNTS, exception.Message);
      }

   }
}
