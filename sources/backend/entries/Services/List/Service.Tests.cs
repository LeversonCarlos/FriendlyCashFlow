using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elesse.Entries.Tests
{
   partial class EntryServiceTests
   {

      [Fact]
      public async void List_WithValidData_MustReturnOkResultWithData()
      {
         var dueDate = DateTime.Now.AddDays(1);
         var pattern = new Patterns.PatternEntity(Shared.EntityID.NewID(), Patterns.enPatternType.Income, Shared.EntityID.NewID(), "Pattern Text");
         var entry = new EntryEntity(pattern, Shared.EntityID.NewID(), dueDate, (decimal)23.45);
         var repository = EntryRepositoryMocker
            .Create()
            .WithList(new EntryEntity[] { entry })
            .Build();
         var service = EntryService.Create(repository);

         var result = await service.ListAsync(dueDate.Year, dueDate.Month);

         Assert.NotNull(result);
         Assert.IsType<OkObjectResult>(result.Result);
         Assert.IsType<EntryEntity[]>((result.Result as OkObjectResult).Value);
         var resultValue = (EntryEntity[])((result.Result as OkObjectResult).Value);
         Assert.NotNull(resultValue);
         Assert.Single(resultValue);
         Assert.Equal(entry.EntryID, resultValue[0].EntryID);

      }

   }
}
