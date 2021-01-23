using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Remove_WithNullParameter_MustReturnBadResult()
      {
         var service = PatternService.Mock(null);

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.RemoveAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_REMOVE_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Remove_WithNonExistingPattern_MustDoNothingAndReturnNull()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Mock(repository);

         var result = await service.RemoveAsync(param);
         Assert.Null(result);
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
         var service = PatternService.Mock(repository);

         var result = await service.RemoveAsync(param);

         Assert.NotNull(result);
         Assert.Equal(param.PatternID, result.PatternID);
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
         var service = PatternService.Mock(repository);

         var result = await service.RemoveAsync(param);

         Assert.NotNull(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

   }
}
