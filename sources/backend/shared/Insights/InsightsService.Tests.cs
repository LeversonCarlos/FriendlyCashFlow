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

      [Theory]
      [MemberData(nameof(GetPropertiesDictionary_WithExpecifiedParameter_MustResultAsExpected_Data))]
      internal void GetPropertiesDictionary_WithExpecifiedParameter_MustResultAsExpected(string[] param, Dictionary<string, string> expected)
      {
         var service = new InsightsService(null);

         var result = service.GetPropertiesDictionary(param);

         Assert.NotNull(result);
         Assert.Equal(expected, result);
      }
      public static IEnumerable<object[]> GetPropertiesDictionary_WithExpecifiedParameter_MustResultAsExpected_Data() =>
         new[] {
            new object[] { new string[] { "one:two:three" }, new Dictionary<string, string> { { "Property", "one:two:three" } } },
            new object[] { new string[] { "one" }, new Dictionary<string, string> { { "Property", "one" } } },
            new object[] { new string[] { "one:two" }, new Dictionary<string, string> { { "one", "two" } } },
            new object[] { new string[] { "one:two", "a:b" }, new Dictionary<string, string> { { "one", "two" }, { "a", "b" } } },
            new object[] { new string[] { "one:two", "a:b", "unsplited" }, new Dictionary<string, string> { { "one", "two" }, { "a", "b" }, { "Property", "unsplited" } } }
      };


   }
}
