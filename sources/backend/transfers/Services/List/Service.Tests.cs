using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elesse.Transfers.Tests
{
   partial class TransferServiceTests
   {

      [Fact]
      public async void List_WithValidData_MustReturnOkResultWithData()
      {
         var date = Shared.Faker.GetFaker().Date.Soon();
         var entity = TransferEntity.Builder().WithDate(date).Build();
         var repository = TransferRepositoryMocker
            .Create()
            .WithList(new TransferEntity[] { entity })
            .Build();
         var service = TransferService.Builder().With(repository).Build();

         var result = await service.ListAsync(date.Year, date.Month);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<TransferEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (TransferEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Single(resultValue);
         Assert.Equal(entity.TransferID, resultValue[0].TransferID);

      }

   }
}
