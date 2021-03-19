using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Elesse.Recurrences.Tests
{
   partial class RecurrenceServiceTests
   {

      [Fact]
      public async void GetNewRecurrence_WithInvalidParameter_MustThrowException()
      {
         var service = RecurrenceService.Builder().Build();

         var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GetNewRecurrenceAsync(null));

         Assert.NotNull(exception);
         Assert.Equal(WARNINGS.INVALID_PROPERTIES, exception.Message);
      }

      [Fact]
      public async void GetNewRecurrence_WithValidParameters_MustReturnNewID()
      {
         var service = RecurrenceService.Builder().Build();
         var properties = RecurrenceProperties.Builder().Build();

         var result = await service.GetNewRecurrenceAsync(properties);

         Assert.NotNull(result);
         Assert.IsAssignableFrom<Shared.EntityID>(result);
      }

   }
}
