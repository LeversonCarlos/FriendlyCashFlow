using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Remove_WithNullParameter_MustReturnBadResult()
      {
         var service = PatternService.Create(null);

         var result = await service.RemoveAsync(null);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_REMOVE_PARAMETER), (result as BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Remove_WithNonExistingPattern_MustDoNothingAndReturnOkResult()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Create(repository);

         var result = await service.RemoveAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkResult>(result);
      }

      [Fact]
      public async void Remove_WithRemainingRowsCountOnPattern_MustUpdateRowsAndDate_AndReturnOk()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         param.RowsCount = 5;
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Create(repository);

         var result = await service.RemoveAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkResult>(result);
      }

      [Fact]
      public async void Remove_WithNoRemainingRowsCountOnPattern_MustDeletePattern_AndReturnOk()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         param.RowsCount = 1;
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Create(repository);

         var result = await service.RemoveAsync(param);

         Assert.NotNull(result);
         Assert.IsType<OkResult>(result);
      }

   }
}
