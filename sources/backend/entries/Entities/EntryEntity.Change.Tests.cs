using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryEntityTests
   {

      [Fact]
      public void Change_WithValidParameters_MustReflectChanges()
      {
         var pattern = Patterns.PatternEntity.Builder().Build();
         var accountID = Shared.EntityID.NewID();
         var dueDate = DateTime.Now.AddDays(1);
         var entryValue = (decimal)23.45;
         var entity = EntryEntity.Builder().Build();

         entity.Change(pattern, accountID, dueDate, entryValue);

         Assert.NotNull(entity);
         Assert.Equal(pattern, entity.Pattern);
         Assert.Equal(accountID, entity.AccountID);
         Assert.Equal(dueDate, entity.DueDate);
         Assert.Equal(entryValue, entity.EntryValue);
      }

   }
}
