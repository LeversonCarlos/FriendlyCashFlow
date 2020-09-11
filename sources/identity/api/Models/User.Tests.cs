using System;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public class UserTests
   {

      [Theory]
      [InlineData((string)null)]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData("123456789 123456789 123456789 12345")]
      [InlineData("123456789 123456789 123456789 1234567")]
      public void NewInstance_WithInvalidUserID_MustThrowException(string userID)
      {
         var exception = Assert.Throws<ArgumentException>(() => new User(userID));

         Assert.NotNull(exception);
         Assert.Equal("WARNING_IDENTITY_INVALID_USERID_DATA", exception.Message);
      }

   }
}