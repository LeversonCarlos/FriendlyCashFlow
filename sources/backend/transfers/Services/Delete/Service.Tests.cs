using System;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferServiceTests
   {

      [Fact]
      public async void Delete_WithNullParameter_MustReturnBadResult()
      {
         var service = TransferService.Builder().Build();

         var result = await service.DeleteAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_DELETE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithInexistingTransfer_MustReturnBadRequest()
      {
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad()
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var result = await service.DeleteAsync((string)Shared.EntityID.MockerID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.TRANSFER_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithExceptionOnRepository_MustReturnBadResult()
      {
         var repositoryException = new Exception(Shared.Faker.GetFaker().Lorem.Sentence(5));
         var entity = TransferEntity.Builder().Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad(entity)
            .WithDelete(repositoryException)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var result = await service.DeleteAsync((string)Shared.EntityID.MockerID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(new Shared.Results(repositoryException), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Delete_WithValidParameters_MustReturnOkResult()
      {
         var entity = TransferEntity.Builder().Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var result = await service.DeleteAsync((string)Shared.EntityID.MockerID());

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
