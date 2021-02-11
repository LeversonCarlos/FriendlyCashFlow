using System;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferServiceTests
   {

      [Fact]
      public async void Update_WithNullParameter_MustReturnBadResult()
      {
         var service = TransferService.Builder().Build();

         var result = await service.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_UPDATE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInexistingTransfer_MustReturnBadRequest()
      {
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad()
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var param = new UpdateVM { TransferID = Shared.EntityID.MockerID() };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.TRANSFER_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInvalidData_MustReturnBadRequest()
      {
         var entity = TransferEntity.Builder().Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var param = new UpdateVM { TransferID = Shared.EntityID.MockerID() };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_EXPENSEACCOUNTID), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithExceptionOnRepository_MustReturnBadResult()
      {
         var repositoryException = new Exception(Shared.Faker.GetFaker().Lorem.Sentence(5));
         var entity = TransferEntity.Builder().Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad(entity)
            .WithUpdate(repositoryException)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var param = new UpdateVM
         {
            TransferID = entity.TransferID,
            ExpenseAccountID = Shared.EntityID.MockerID(),
            IncomeAccountID = Shared.EntityID.MockerID(),
            Date = entity.Date,
            Value = entity.Value
         };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new Shared.Results(repositoryException), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithValidData_MustReturnOkRequest()
      {
         var entity = TransferEntity.Builder().Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var param = new UpdateVM
         {
            TransferID = entity.TransferID,
            ExpenseAccountID = Shared.EntityID.MockerID(),
            IncomeAccountID = Shared.EntityID.MockerID(),
            Date = entity.Date,
            Value = entity.Value
         };
         var result = await service.UpdateAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
