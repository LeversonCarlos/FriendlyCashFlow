using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Patterns.Tests
{
   partial class PatternServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameters_MustReturnBadResult()
      {
         var service = PatternService.Create(null);

         var result = await service.AddAsync(null);

         Assert.Null(result);
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
         var service = PatternService.Create(repository);

         var result = await service.AddAsync(param);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<IPatternEntity>(result);
      }

   }
}
