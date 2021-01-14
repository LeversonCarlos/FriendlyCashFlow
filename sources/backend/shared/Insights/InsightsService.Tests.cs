using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Elesse.Shared.Tests
{
   public class InsightsServiceTests
   {

      [Fact]
      internal void GetPropertiesDictionary_WithNullParameter_MustResultNull()
      {
         var service = new InsightsService(null);

         var result = service.GetPropertiesDictionary(null);

         Assert.Null(result);
      }

   }
}
