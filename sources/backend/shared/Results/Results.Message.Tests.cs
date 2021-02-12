using System;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elesse.Shared.Tests
{
   partial class ResultsTests
   {

      [Fact]
      public void Info_WithData_MustResultBadRequestResult()
      {
         var resource = Faker.GetFaker().Commerce.Product().ToLower();
         var message1 = Faker.GetFaker().Lorem.Sentence(10);
         var message2 = Faker.GetFaker().Lorem.Sentence(10);

         var result = Results.Info(resource, message1, message2);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.NotNull(result.Value);
         Assert.IsType<Results>(result.Value);
         var resultValue = result.Value as Results;
         Assert.Equal(enResultType.Info, resultValue.Type);
         Assert.Equal($"{resource}.{message1}", resultValue.Messages[0]);
         Assert.Equal($"{resource}.{message2}", resultValue.Messages[1]);
      }

      [Fact]
      public void Warning_WithData_MustResultBadRequestResult()
      {
         var resource = Faker.GetFaker().Commerce.Product().ToLower();
         var message1 = Faker.GetFaker().Lorem.Sentence(10);
         var message2 = Faker.GetFaker().Lorem.Sentence(10);

         var result = Results.Warning(resource, message1, message2);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.NotNull(result.Value);
         Assert.IsType<Results>(result.Value);
         var resultValue = result.Value as Results;
         Assert.Equal(enResultType.Warning, resultValue.Type);
         Assert.Equal($"{resource}.{message1}", resultValue.Messages[0]);
         Assert.Equal($"{resource}.{message2}", resultValue.Messages[1]);
      }

      [Fact]
      public void Exception_WithData_MustResultBadRequestResult()
      {
         var exception = new Exception(Faker.GetFaker().Lorem.Sentence(10));

         var result = Results.Exception(exception);

         Assert.NotNull(result);
         Assert.IsType<BadRequestObjectResult>(result);
         Assert.NotNull(result.Value);
         Assert.IsType<Results>(result.Value);
         var resultValue = result.Value as Results;
         Assert.Equal(enResultType.Exception, resultValue.Type);
         Assert.Equal(exception.ToString(), resultValue.Details);
      }

   }
}
