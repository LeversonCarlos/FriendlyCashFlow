using Moq;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Load_WithNullParameters_MustThrowException()
      {
         var service = PatternService.Builder().Build();

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.LoadAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_LOAD_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Load_WithNullCategory_MustThrowException()
      {
         var service = PatternService.Builder().Build();
         var param = new Mock<IPatternEntity>().Object;

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.LoadAsync(param));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_CATEGORYID, exception.Message);
      }

      [Fact]
      public async void Load_WithNullText_MustThrowException()
      {
         var service = PatternService.Builder().Build();
         var patternEntityMocker = new Mock<IPatternEntity>();
         patternEntityMocker.SetupGet(x => x.CategoryID).Returns(Shared.EntityID.MockerID());
         var param = patternEntityMocker.Object;

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.LoadAsync(param));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_TEXT, exception.Message);
      }

      [Fact]
      public async void Load_WithExistingPattern_MustResultPatternID()
      {
         var param = PatternEntity.Builder().Build();
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.LoadAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

      [Fact]
      public async void Load_WithNonExistingPattern_MustCreateRecord_AndReturnPatternID()
      {
         var param = PatternEntity.Builder().Build();
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Builder().With(repository).Build();

         var result = await service.LoadAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
      }

   }
}
