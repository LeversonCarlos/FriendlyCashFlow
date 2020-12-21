using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Identity.Tests
{
   public class UserEntityTests
   {


      [Fact]
      public void Constructor_WithValidParameters_MustResultValidInstance()
      {
         var userName = "userName";
         var password = "password";
         var user = new UserEntity(userName, password);

         Assert.NotNull(user);
         Assert.NotEmpty(user.UserID);
         Assert.Equal(36, user.UserID.Length);
         Assert.Equal(userName, user.UserName);
         Assert.Equal(password, user.Password);
      }

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, string userID, string userName, string password)
      {
         var exception = Assert.Throws<ArgumentException>(() => new UserEntity(userID, userName, password));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERID_PARAMETER, (string)null, "UserName", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERID_PARAMETER, "", "UserName", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERID_PARAMETER, " ", "UserName", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERID_PARAMETER, "123456789_123456789_123456789_12345", "UserName", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERID_PARAMETER, "123456789_123456789_123456789_1234567", "UserName", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERNAME_PARAMETER, "123456789_123456789_123456789_123456", (string)null, "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERNAME_PARAMETER, "123456789_123456789_123456789_123456", "", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERNAME_PARAMETER, "123456789_123456789_123456789_123456", " ", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERNAME_PARAMETER, "123456789_123456789_123456789_123456", "1234567", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_USERNAME_PARAMETER, "123456789_123456789_123456789_123456", "123456789_123456789_123456789_123456789_123456789_1", "Password" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER, "123456789_123456789_123456789_123456", "UserName", (string)null },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER, "123456789_123456789_123456789_123456", "UserName", "" },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER, "123456789_123456789_123456789_123456", "UserName", " " },
            new object[] { UserEntity.WARNING_IDENTITY_INVALID_PASSWORD_PARAMETER, "123456789_123456789_123456789_123456", "UserName", "1234" }
         };

   }
}