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

      internal void EqualOperator_WithBothNullValues_MustResultTrue()
      {
         ValueObjectTest first = null;
         ValueObjectTest second = null;

         var result = first == second;
         Assert.True(result);
      }

      [Theory]
      [MemberData(nameof(EqualOperator_WithNullValues_MustResultFalse_Data))]
      internal void EqualOperator_WithOneNullValue_MustResultFalse(ValueObjectTest first, ValueObjectTest second)
      {
         var result = first == second;
         Assert.False(result);
      }
      public static IEnumerable<object[]> EqualOperator_WithNullValues_MustResultFalse_Data() =>
         new[] {
            new object[] { new ValueObjectTest { One = "One", Two = "Two" }, (ValueObjectTest)null },
            new object[] { (ValueObjectTest)null, new ValueObjectTest { One = "One", Two = "Two" } }
      };

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
