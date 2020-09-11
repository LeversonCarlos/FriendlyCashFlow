using System;
using System.Collections.Generic;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public class UserTests
   {


      [Fact]
      public void Constructor_WithValidParameters_MustResultValidInstance()
      {
         var user = new User("UserName");

         Assert.NotNull(user);
         Assert.NotEmpty(user.UserID);
         Assert.Equal(36, user.UserID.Length);
      }

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, string userID, string userName)
      {
         var exception = Assert.Throws<ArgumentException>(() => new User(userID, userName));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", (string)null, "UserName" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "", "UserName" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", " ", "UserName" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "123456789_123456789_123456789_12345", "UserName" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "123456789_123456789_123456789_1234567", "UserName" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", (string)null },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", "" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", " " },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", "1234567" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", "123456789_123456789_123456789_123456789_123456789_1" }
         };

   }
}