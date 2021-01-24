using System;
using Xunit;

namespace Elesse.Entries.Tests
{
   partial class EntryEntityTests
   {

      [Fact]
      public void Create_WithValidParameters_MustResultInstance()
      {
         var pattern = Patterns.PatternEntity.Builder().Build();
         var accountID = Shared.EntityID.MockerID();
         var dueDate = Shared.Faker.GetFaker().Date.Soon();
         var entryValue = Shared.Faker.GetFaker().Random.Decimal(0, 10000);

         var result = EntryEntity.Create(pattern, accountID, dueDate, entryValue);

         Assert.NotNull(result);
         Assert.NotNull(result.EntryID);
         Assert.Equal(36, ((string)result.EntryID).Length);
         Assert.Equal(pattern, result.Pattern);
         Assert.Equal(accountID, result.AccountID);
         Assert.Equal(dueDate, result.DueDate);
         Assert.Equal(entryValue, result.EntryValue);
      }

   }
}
