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
         var value = (decimal)23.45;
         var entity = EntryEntity.Builder().Build();

         entity.Change(pattern, accountID, dueDate, value);

         Assert.NotNull(entity);
         Assert.Equal(pattern, entity.Pattern);
         Assert.Equal(accountID, entity.AccountID);
         Assert.Equal(dueDate, entity.DueDate);
         Assert.Equal(value, entity.Value);
      }

   }
}
