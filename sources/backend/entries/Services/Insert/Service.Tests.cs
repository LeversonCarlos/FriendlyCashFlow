using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Insert_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Builder().Build();

         var result = await service.InsertAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_INSERT_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithInvalidPattern_MustReturnBadResult()
      {
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(new ArgumentException(Patterns.WARNINGS.INVALID_INCREASE_PARAMETER))
            .Build();
         var service = EntryService.Mock(patternService);

         var result = await service.InsertAsync(new InsertVM { });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(Patterns.WARNINGS.INVALID_INCREASE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithInvaidAccount_MustReturnBadResult()
      {
         var pattern = Patterns.PatternEntity.Builder().Build();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(pattern)
            .Build();
         var service = EntryService.Mock(patternService);

         var param = new InsertVM { Pattern = pattern, AccountID = null };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_ACCOUNTID), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Insert_WithValidParametersAndWithoutPayment_MustReturnOkResult()
      {
         var pattern = Patterns.PatternEntity.Builder().Build();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(pattern)
            .Build();
         var service = EntryService.Mock(patternService);

         var param = new InsertVM { Pattern = pattern, AccountID = Shared.EntityID.NewID(), DueDate = DateTime.Now.AddDays(1), Value = (decimal)12.34 };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

      [Fact]
      public async void Insert_WithValidParametersAndWithPayment_MustReturnOkResult()
      {
         var pattern = Patterns.PatternEntity.Builder().Build();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithIncrease(pattern)
            .Build();
         var service = EntryService.Mock(patternService);

         var param = new InsertVM
         {
            Pattern = pattern,
            AccountID = Shared.EntityID.NewID(),
            DueDate = DateTime.Now.AddDays(1),
            Value = (decimal)12.34,
            Paid = true,
            PayDate = DateTime.Now
         };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

      [Fact]
      public async void Insert_WithValidParametersAndWithThreeRecurrences_MustInsertThreeEntriesAndReturnOkResult()
      {
         var faker = Shared.Faker.GetFaker();
         var pattern = Patterns.PatternEntity.Builder().Build();
         var patternService = Patterns.Tests.PatternServiceMocker
            .Create()
            .WithRetrieve(pattern)
            .WithIncrease(pattern)
            .Build();
         var recurrenceID = Shared.EntityID.MockerID();
         var recurrenceService = Recurrences.RecurrenceService.Mocker()
            .WithGetNewRecurrence(recurrenceID)
            .Build();
         var service = EntryService.Builder()
            .With(patternService)
            .With(recurrenceService)
            .Build();

         var param = new InsertVM
         {
            Pattern = pattern,
            AccountID = Shared.EntityID.MockerID(),
            DueDate = faker.Date.Soon(10),
            Value = faker.Random.Decimal(10, 1000),
            Recurrence = new InsertRecurrenceVM { Type = Recurrences.enRecurrenceType.Monthly, TotalOccurrences = 3 },
            Paid = false
         };
         var result = await service.InsertAsync(param);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
