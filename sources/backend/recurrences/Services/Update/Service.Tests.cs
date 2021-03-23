using System;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   partial class RecurrenceServiceTests
   {

      [Fact]
      public async void Update_WithNullParameters_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();

         var result = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(null));

         Assert.NotNull(result);
         Assert.Equal(WARNINGS.INVALID_UPDATE_PARAMETER, result.Message);
      }

      [Fact]
      public async void Update_WithNullRecurrence_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();
         var param = new Moq.Mock<IRecurrenceEntity>().Object;

         var result = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(param));

         Assert.NotNull(result);
         Assert.Equal(WARNINGS.INVALID_RECURRENCEID, result.Message);
      }

      [Fact]
      public async void Update_WithNullProperties_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();
         var patternEntityMocker = new Moq.Mock<IRecurrenceEntity>();
         patternEntityMocker.SetupGet(x => x.RecurrenceID).Returns(Shared.EntityID.MockerID());
         var param = patternEntityMocker.Object;

         var result = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(param));

         Assert.NotNull(result);
         Assert.Equal(WARNINGS.INVALID_PROPERTIES, result.Message);
      }

      [Fact]
      public async void Update_WithNotExistingRecurrence_MustThrowException()
      {
         var param = RecurrenceEntity.Builder().Build();
         var repository = RecurrenceRepository.Mocker()
            .WithLoad(new NullReferenceException(WARNINGS.RECURRENCE_NOT_FOUND))
            .Build();
         var service = RecurrenceService.Builder().With(repository).Build();

         var result = await Assert.ThrowsAsync<NullReferenceException>(() => service.UpdateAsync(param));

         Assert.NotNull(result);
         Assert.Equal(WARNINGS.RECURRENCE_NOT_FOUND, result.Message);
      }

      [Fact]
      public async void Update_WithExistingRecurrence_MustCallRepositoryWithChangedData()
      {
         var param = RecurrenceEntity.Builder().Build();
         var changedParam = RecurrenceEntity.Restore(param.RecurrenceID, RecurrenceProperties.Builder().Build());
         Moq.Mock<IRecurrenceRepository> repositoryMocker = null;
         var repository = RecurrenceRepository.Mocker()
            .With(mock => repositoryMocker = mock)
            .WithLoad(changedParam)
            .Build();
         var service = RecurrenceService.Builder().With(repository).Build();

         await service.UpdateAsync(param);

         repositoryMocker.Verify(m => m.UpdateAsync(changedParam));
      }

   }
}
