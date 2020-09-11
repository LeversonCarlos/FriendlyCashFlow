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
         var user = new User("UserName", "Password");

         Assert.NotNull(user);
         Assert.NotEmpty(user.UserID);
         Assert.Equal(36, user.UserID.Length);
      }

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, string userID, string userName, string password)
      {
         var exception = Assert.Throws<ArgumentException>(() => new User(userID, userName, password));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", (string)null, "UserName", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "", "UserName", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", " ", "UserName", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "123456789_123456789_123456789_12345", "UserName", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "123456789_123456789_123456789_1234567", "UserName", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", (string)null, "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", "", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", " ", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", "1234567", "Password" },
            new object[] { "WARNING_IDENTITY_INVALID_USERNAME_DATA", "123456789_123456789_123456789_123456", "123456789_123456789_123456789_123456789_123456789_1", "Password" }, 
            new object[] { "WARNING_IDENTITY_INVALID_PASSWORD_DATA", "123456789_123456789_123456789_123456", "UserName", (string)null },
            new object[] { "WARNING_IDENTITY_INVALID_PASSWORD_DATA", "123456789_123456789_123456789_123456", "UserName", "" },
            new object[] { "WARNING_IDENTITY_INVALID_PASSWORD_DATA", "123456789_123456789_123456789_123456", "UserName", " " },
            new object[] { "WARNING_IDENTITY_INVALID_PASSWORD_DATA", "123456789_123456789_123456789_123456", "UserName", "1234" }
         };

   }
}