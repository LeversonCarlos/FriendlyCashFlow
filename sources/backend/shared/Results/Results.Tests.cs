using System;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Shared.Tests
{
   public partial class ResultsTests
   {

      [Fact]
      public void TwoMessageInstances_WithSameData_MustHaveSaveHashCode()
      {
         var resultType = Faker.GetFaker().PickRandom<enResultType>();
         var resultMessage = Faker.GetFaker().Lorem.Sentence(10);
         var param1 = new Results(resultType, new string[] { resultMessage });
         var param2 = new Results(resultType, new string[] { resultMessage });

         var result = param1 == param2;

         Assert.True(result);
      }

      [Fact]
      public void TwoExceptionInstances_WithSameData_MustHaveSaveHashCode()
      {
         var exception = new Exception(Faker.GetFaker().Lorem.Sentence(10));
         var param1 = new Results(exception);
         var param2 = new Results(exception);

         var result = param1 == param2;

         Assert.True(result);
      }

   }
}
