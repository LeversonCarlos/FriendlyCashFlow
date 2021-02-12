using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elesse.Transfers.Tests
{
   partial class TransferServiceTests
   {

      [Fact]
      public async void Load_WithNullParameter_MustReturnBadResult()
      {
         var service = TransferService.Builder().Build();

         var result = await service.LoadAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(Warning(WARNINGS.INVALID_LOAD_PARAMETER), (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Load_WithValidData_MustReturnOkResultWithData()
      {
         var entity = TransferEntity.Builder().Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var result = await service.LoadAsync((string)entity.TransferID);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<TransferEntity>((result.Result as OkObjectResult).Value);
         var resultValue = (TransferEntity)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Equal(entity.TransferID, resultValue.TransferID);

      }

   }
}
