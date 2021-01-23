using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameters_MustThrowException()
      {
         var service = PatternService.Mock(null);

         var exception = await Assert.ThrowsAsync<System.ArgumentException>(() => service.AddAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_ADD_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Insert_WithExistingPattern_MustUpdateRowsAndDate_AndReturnPatternID()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern(new IPatternEntity[] { param })
            .Build();
         var service = PatternService.Mock(repository);

         var result = await service.AddAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
         Assert.Equal(param.PatternID, result.PatternID);
      }

      [Fact]
      public async void Insert_WithNonExistingPattern_MustCreateRecord_AndReturnPatternID()
      {
         var param = new PatternEntity(enPatternType.Expense, Shared.EntityID.NewID(), "Pattern Text");
         var repository = PatternRepositoryMocker
            .Create()
            .WithLoadPattern()
            .Build();
         var service = PatternService.Mock(repository);

         var result = await service.AddAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
      }

   }
}
