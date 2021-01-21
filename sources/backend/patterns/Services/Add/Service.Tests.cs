using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameter_MustReturnBadResult()
      {
         var service = PatternService.Create(null);

         var result = await service.AddAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result.Result);
         Assert.Equal(Warning(WARNINGS.INVALID_ADD_PARAMETER), (result.Result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithExistingPattern_MustUpdateRowsAndDate_AndReturnPatternID()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Create(repository);

         var result = await service.AddAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<Shared.EntityID>((result.Result as OkObjectResult).Value);
         var resultValue = (Shared.EntityID)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Equal(param.PatternID, resultValue);
      }

      [Fact]
      public async void Insert_WithNonExistingPattern_MustCreateRecord_AndReturnPatternID()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Create(repository);

         var result = await service.AddAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<Shared.EntityID>((result.Result as OkObjectResult).Value);
         var resultValue = (Shared.EntityID)((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
      }

   }
}
