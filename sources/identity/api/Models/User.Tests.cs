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
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "123456789 123456789 123456789 12345", "UserName" },
            new object[] { "WARNING_IDENTITY_INVALID_USERID_DATA", "123456789 123456789 123456789 1234567", "UserName" }
         };

   }
}