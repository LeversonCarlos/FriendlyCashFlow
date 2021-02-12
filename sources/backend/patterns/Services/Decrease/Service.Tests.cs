using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Decrease_WithNullParameter_MustReturnBadResult()
      {
         var service = PatternService.Builder().Build();

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.DecreaseAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_DECREASE_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Decrease_WithNonExistingPattern_MustDoNothingAndReturnNull()
      {
         var param = PatternEntity.Builder().Build();
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.DecreaseAsync(param);
         Assert.Null(result);
      }

      [Fact]
      public async void Decrease_WithRemainingRowsCountOnPattern_MustUpdateRowsAndDate_AndReturnOk()
      {
         var param = PatternEntity.Builder().Build();
         param.RowsCount = 5;
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.DecreaseAsync(param);

         Assert.NotNull(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

      [Fact]
      public async void Decrease_WithNoRemainingRowsCountOnPattern_MustDeletePattern_AndReturnOk()
      {
         var param = PatternEntity.Builder().Build();
         param.RowsCount = 1;
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.DecreaseAsync(param);

         Assert.NotNull(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

   }
}
