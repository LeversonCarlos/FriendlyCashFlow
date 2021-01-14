using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Elesse.Shared.Tests
{
   public class InsightsServiceTests
   {

      [Theory]
      [MemberData(nameof(GetPropertiesDictionary_WithNullParameter_MustResultNull_Data))]
      internal void GetPropertiesDictionary_WithNullParameter_MustResultNull(string[] propertyList)
      {
         var service = new InsightsService(null);

         var result = service.GetPropertiesDictionary(propertyList);

         Assert.Null(result);
      }
      public static IEnumerable<object[]> GetPropertiesDictionary_WithNullParameter_MustResultNull_Data() =>
         new[] {
            new object[] { null },
            new object[] { new string[] { } }
      };

      [Fact]
      internal void GetPropertiesDictionary_WithOneParameterAndThreeSplits_MustResultAsExpected()
      {
         var service = new InsightsService(null);
         var param = new string[] { "one:two:three" };
         var expected = new Dictionary<string, string> { { "Property", "one:two:three" } };

         var result = service.GetPropertiesDictionary(param);

         Assert.NotNull(result);
         Assert.Equal(expected, result);
      }

   }
}
