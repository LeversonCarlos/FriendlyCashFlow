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
      public async void Update_WithNotFoundRecurrence_MustThrowException()
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

      /*
      [Fact]
      public async void Update_WithInvalidParameter_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();

         var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.InsertAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PROPERTIES, exception.Message);
      }

      [Fact]
      public async void Insert_WithValidParameters_MustReturnNewID()
      {
         var service = RecurrenceService.Builder().Build();
         var properties = RecurrenceProperties.Builder().Build();

         var result = await service.InsertAsync(properties);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<Shared.EntityID>(result);
      }
      */

   }
}
