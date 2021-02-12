using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void List_WithValidData_MustReturnOkResultWithData()
      {
         var entity = PatternEntity.Builder().Build();
         var repository = PatternRepositoryMocker
            .Create()
            .WithList(new PatternEntity[] { entity })
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.ListAsync();

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<PatternEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (PatternEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Single(resultValue);
         Assert.Equal(entity.PatternID, resultValue[0].PatternID);

      }

   }
}
