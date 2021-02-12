using System;
using Xunit;

namespace Elesse.Shared.Tests
{
   public class EntityIDTests
   {

      [Fact]
      internal void NewID_MustSetValidGuid()
      {

         var result = EntityID.NewID();

         Assert.NotNull(result);
         Assert.IsType<Guid>(result.Value);
      }

      [Theory]
      [InlineData((string)null)]
      [InlineData("")]
      [InlineData(" ")]
      internal void Parse_WithEmptyString_MustThrowException(string guidString)
      {

         var result = Assert.Throws<ArgumentException>(() => EntityID.Parse(guidString));

         Assert.NotNull(result);
         Assert.IsType<ArgumentException>(result);
         Assert.Equal("The string argument to parse an EntityID type cannot be empty", result.Message);
      }

      [Fact]
      internal void Parse_WithInvalidGuid_MustThrowException()
      {

         var result = Assert.Throws<ArgumentException>(() => EntityID.Parse("some invalid guid"));

         Assert.NotNull(result);
         Assert.IsType<ArgumentException>(result);
         Assert.Equal("The string argument [some invalid guid] received to parse an EntityID type is invalid", result.Message);
      }

      [Fact]
      internal void TryParse_WithValidGuid_MustCreateInstance()
      {

         var guidString = Guid.NewGuid().ToString();
         EntityID.TryParse(guidString, out EntityID result);

         Assert.NotNull(result);
         Assert.Equal(guidString, result.ToString());
      }

      [Fact]
      internal void TryParse_WithInvalidGuid_MustResultFalse()
      {

         var result = EntityID.TryParse("some invalid guid", out EntityID dummy);

         Assert.False(result);
      }

      [Fact]
      internal void ExplicitOperator_OneEqualValuesOfDifferentTypes_MustResultTrue()
      {
         var guid = Guid.NewGuid().ToString();

         var first = (EntityID)guid;
         var second = EntityID.Parse(guid);

         Assert.Equal(first.GetHashCode(), second.GetHashCode());
         Assert.Equal((string)first, (string)second);
      }

      [Fact]
      internal void ToLong_MustReduceToExpectedLong()
      {
         var entity = EntityID.Parse("f7a3e0a8-6b74-4c7e-90c3-522e1dc10b02");
         long expected = 36434487158610448;

         var result = entity.ToLong();

         Assert.Equal(expected, result);
      }

   }
}
