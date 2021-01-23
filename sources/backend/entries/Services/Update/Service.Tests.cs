using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void Update_WithNullParameter_MustReturnBadResult()
      {
         var service = EntryService.Mock();

         var result = await service.UpdateAsync(null);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_UPDATE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInexistingEntry_MustReturnBadRequest()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad()
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.UpdateAsync(new UpdateVM { EntryID = Shared.EntityID.NewID() });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.ENTRY_NOT_FOUND), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithExceptionWhenChangingPattern_MustReturnBadRequest()
      {
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(EntryEntity.Mock())
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.UpdateAsync(new UpdateVM { EntryID = Shared.EntityID.NewID() });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(Patterns.WARNINGS.INVALID_INCREASE_PARAMETER), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithInvalidAccount_MustReturnBadRequest()
      {
         var entity = EntryEntity.Mock();
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = EntryService.Mock(repository);

         var result = await service.UpdateAsync(new UpdateVM { EntryID = Shared.EntityID.NewID(), Pattern = entity.Pattern });

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
         Assert.Equal(Warning(WARNINGS.INVALID_ACCOUNTID), (result as Microsoft.AspNetCore.Mvc.BadRequestObjectResult).Value);
      }

      [Fact]
      public async void Update_WithValidDataAndEmptyPayment_MustReturnOkRequest()
      {
         var entity = EntryEntity.Mock();
         entity.SetPayment(DateTime.Now, entity.EntryValue);
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = EntryService.Mock(repository);

         var updateVM = new UpdateVM
         {
            EntryID = Shared.EntityID.NewID(),
            Pattern = entity.Pattern,
            AccountID = entity.AccountID,
            DueDate = entity.DueDate,
            EntryValue = entity.EntryValue
         };
         var result = await service.UpdateAsync(updateVM);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

      [Fact]
      public async void Update_WithValidDataAndWithPayment_MustReturnOkRequest()
      {
         var entity = EntryEntity.Mock();
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = EntryService.Mock(repository);

         var updateVM = new UpdateVM
         {
            EntryID = Shared.EntityID.NewID(),
            Pattern = entity.Pattern,
            AccountID = entity.AccountID,
            DueDate = entity.DueDate,
            EntryValue = entity.EntryValue,
            Paid = true,
            PayDate = DateTime.Now.AddDays(3)
         };
         var result = await service.UpdateAsync(updateVM);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

      [Fact]
      public async void Update_WithValidDataAndWithChangedPattern_MustReturnOkRequest()
      {
         var entity = EntryEntity.Mock();
         var repository = EntryRepositoryMocker
            .Create()
            .WithLoad(entity)
            .Build();
         var service = EntryService.Mock(repository);

         var updateVM = new UpdateVM
         {
            EntryID = Shared.EntityID.NewID(),
            Pattern = Patterns.PatternEntity.Mock(),
            AccountID = entity.AccountID,
            DueDate = entity.DueDate,
            EntryValue = entity.EntryValue,
            Paid = true,
            PayDate = DateTime.Now.AddDays(3)
         };
         var result = await service.UpdateAsync(updateVM);

         Assert.NotNull(result);
         Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
      }

   }
}
