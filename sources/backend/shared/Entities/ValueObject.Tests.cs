using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Elesse.Shared.Tests
{
   public class ValueObjectTests
   {

      [Fact]
      internal void EqualOperator_WithEqualValues_MustResultTrue()
      {
         var first = new ValueObjectTest { One = "One", Two = "Two" };
         var second = new ValueObjectTest { Two = "Two", One = "One" };

         var result = first == second;

         Assert.True(result);
      }

      [Fact]
      internal void EqualOperator_WithDifferentValues_MustResultFalse()
      {
         var first = new ValueObjectTest { One = "One", Two = "Two" };
         var second = new ValueObjectTest { One = "One" };

         var result = first == second;

         Assert.False(result);
      }

      [Fact]
      internal void NotEqualOperator_WithEqualValues_MustResultFalse()
      {
         var first = new ValueObjectTest { One = "One", Two = "Two" };
         var second = new ValueObjectTest { Two = "Two", One = "One" };

         var result = first != second;

         Assert.False(result);
      }

      [Fact]
      internal void NotEqualOperator_WithDifferentValues_MustResultTrue()
      {
         var first = new ValueObjectTest { One = "One", Two = "Two" };
         var second = new ValueObjectTest { One = "One" };

         var result = first != second;

         Assert.True(result);
      }

      [Fact]
      internal void Clone_HashCode_MustBeEqual()
      {
         var first = new ValueObjectTest { One = "One", Two = "Two" };
         var second = first.GetCopy();

         Assert.Equal(first.GetHashCode(), second.GetHashCode());
      }

      internal class ValueObjectTest : ValueObject
      {
         public string One { get; set; }
         public string Two { get; set; }
         protected override IEnumerable<object> GetAtomicValues()
         {
            yield return One;
            yield return Two;
         }
      }

   }
}
