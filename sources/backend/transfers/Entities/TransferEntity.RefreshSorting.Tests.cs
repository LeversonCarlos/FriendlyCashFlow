using System;
using Xunit;

namespace Elesse.Transfers.Tests
{
   partial class TransferEntityTests
   {

      [Fact]
      public void RefreshSorting_MustReflectChangesBasedOnDate()
      {
         var entity = TransferEntity.Builder().Build();

         entity.RefreshSorting();

         Assert.True(entity.Sorting > 0);
      }

   }
}
