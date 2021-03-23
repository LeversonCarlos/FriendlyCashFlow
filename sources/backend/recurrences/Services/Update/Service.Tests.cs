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

         var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_UPDATE_PARAMETER, exception.Message);
      }

      [Fact]
      public async void Increase_WithNullCategory_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();
         var param = new Moq.Mock<IRecurrenceEntity>().Object;

         var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(param));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_RECURRENCEID, exception.Message);
      }

      [Fact]
      public async void Increase_WithNullText_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();
         var patternEntityMocker = new Moq.Mock<IRecurrenceEntity>();
         patternEntityMocker.SetupGet(x => x.RecurrenceID).Returns(Shared.EntityID.MockerID());
         var param = patternEntityMocker.Object;

         var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(param));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PROPERTIES, exception.Message);
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
