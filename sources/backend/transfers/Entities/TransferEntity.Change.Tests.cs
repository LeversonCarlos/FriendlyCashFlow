using System;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferEntityTests
   {

      [Fact]
      public void Change_WithValidParameters_MustReflectChanges()
      {
         var expenseAccountID = Shared.EntityID.MockerID();
         var incomeAccountID = Shared.EntityID.MockerID();
         var date = Shared.Faker.GetFaker().Date.Soon();
         var value = Shared.Faker.GetFaker().Random.Decimal(0, 10000);
         var entity = TransferEntity.Builder().Build();

         entity.Change(expenseAccountID, incomeAccountID, date, value);

         Assert.Equal(expenseAccountID, entity.ExpenseAccountID);
         Assert.Equal(incomeAccountID, entity.IncomeAccountID);
         Assert.Equal(date, entity.Date);
         Assert.Equal(value, entity.Value);
      }

   }
}
