using System;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameter_MustReturnBadResult()
      {
         var service = TransferService.Builder().Build();

         var result = await service.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_INSERT_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithInvalidExpenseAccount_MustReturnBadResult()
      {
         var service = TransferService.Builder().Build();

         var param = new InsertVM { ExpenseAccountID = null };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_EXPENSEACCOUNTID), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithExceptionOnRepository_MustReturnBadResult()
      {
         var repositoryException = new Exception(Shared.Faker.GetFaker().Lorem.Sentence(5));
         var repository = TransferRepositoryMocker
            .Create()
            .WithInsert(repositoryException)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var param = new InsertVM
         {
            ExpenseAccountID = Shared.EntityID.MockerID(),
            IncomeAccountID = Shared.EntityID.MockerID(),
            Date = Shared.Faker.GetFaker().Date.Soon(),
            Value = Shared.Faker.GetFaker().Random.Decimal(0, 10000)
         };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new Shared.Results(repositoryException), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithValidParameters_MustReturnOkResult()
      {
         var service = TransferService.Builder().Build();

         var param = new InsertVM
         {
            ExpenseAccountID = Shared.EntityID.MockerID(),
            IncomeAccountID = Shared.EntityID.MockerID(),
            Date = Shared.Faker.GetFaker().Date.Soon(),
            Value = Shared.Faker.GetFaker().Random.Decimal(0, 10000)
         };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
