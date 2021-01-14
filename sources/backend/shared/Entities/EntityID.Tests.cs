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
      internal void Parse_WithValidGuid_MustCreateInstance()
      {

         var guidString = Guid.NewGuid().ToString();
         var result = EntityID.Parse(guidString);

         Assert.NotNull(result);
         Assert.Equal(guidString, result.ToString());
      }

   }
}
