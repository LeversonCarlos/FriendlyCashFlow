using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Increase_WithNullParameters_MustThrowException()
      {
         var service = PatternService.Mock(null);

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.IncreaseAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_INCREASE_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Increase_WithExistingPattern_MustUpdateRowsAndDate_AndReturnPatternID()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Mock(repository);

         var result = await service.IncreaseAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

      [Fact]
      public async void Increase_WithNonExistingPattern_MustCreateRecord_AndReturnPatternID()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Mock(repository);

         var result = await service.IncreaseAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
      }

   }
}