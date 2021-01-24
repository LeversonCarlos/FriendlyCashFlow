using Moq;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Increase_WithNullParameters_MustThrowException()
      {
         var service = PatternService.Builder().Build();

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.IncreaseAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_INCREASE_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Increase_WithNullCategory_MustThrowException()
      {
         var service = PatternService.Builder().Build();
         var param = new Mock<IPatternEntity>().Object;

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.IncreaseAsync(param));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_CATEGORYID, exception.Message);
      }

      [Fact]
      public async void Increase_WithNullText_MustThrowException()
      {
         var service = PatternService.Builder().Build();
         var patternEntityMocker = new Mock<IPatternEntity>();
         patternEntityMocker.SetupGet(x => x.CategoryID).Returns(Shared.EntityID.MockerID());
         var param = patternEntityMocker.Object;

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.IncreaseAsync(param));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_TEXT, exception.Message);
      }

      [Fact]
      public async void Increase_WithExistingPattern_MustUpdateRowsAndDate_AndReturnPatternID()
      {
         var param = PatternEntity.Builder().Build();
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.IncreaseAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

      [Fact]
      public async void Increase_WithNonExistingPattern_MustCreateRecord_AndReturnPatternID()
      {
         var param = PatternEntity.Builder().Build();
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.IncreaseAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
      }

   }
}
