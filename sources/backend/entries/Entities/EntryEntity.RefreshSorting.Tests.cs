using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryEntityTests
   {

      [Fact]
      public void RefreshSorting_WithoutPayment_MustReflectChangesBasedOnDueDate()
      {
         var entity = EntryEntity.Builder().Build();

         entity.RefreshSorting();

         Assert.Equal(entity.DueDate, entity.SearchDate);
         Assert.True(entity.Sorting > 0);
      }

      [Fact]
      public void RefreshSorting_WithPayment_MustReflectChangesBasedOnPayDate()
      {
         var entity = EntryEntity.Builder().WithPayDate(Shared.Faker.GetFaker().Date.Soon()).Build();

         entity.RefreshSorting();

         Assert.Equal(entity.PayDate, entity.SearchDate);
         Assert.True(entity.Sorting > 0);
      }

   }
}
