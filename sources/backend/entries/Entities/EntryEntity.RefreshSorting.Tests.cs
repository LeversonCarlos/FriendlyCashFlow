using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryEntityTests
   {

      [Fact]
      public void RefreshSorting_WithoutPayment_MustReflectChangesBasedOnDueDate()
      {
         var entity = EntryEntity.Create(Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.Now.AddDays(1), (decimal)54.32);

         entity.RefreshSorting();

         Assert.Equal(entity.DueDate, entity.SearchDate);
         Assert.True(entity.Sorting > 0);
      }

      [Fact]
      public void RefreshSorting_WithPayment_MustReflectChangesBasedOnPayDate()
      {
         var entity = EntryEntity.Create(Patterns.PatternEntity.Builder().Build(), Shared.EntityID.NewID(), DateTime.Now.AddDays(1), (decimal)54.32);
         entity.SetPayment(DateTime.Now.AddDays(2), entity.EntryValue);

         entity.RefreshSorting();

         Assert.Equal(entity.PayDate, entity.SearchDate);
         Assert.True(entity.Sorting > 0);
      }

   }
}