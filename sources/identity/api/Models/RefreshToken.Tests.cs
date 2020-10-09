using System;
using System.Collections.Generic;
using Xunit;

namespace FriendlyCashFlow.Identity.Tests
{
   public class RefreshTokenTests
   {

      [Fact]
      public void Create_WithValidParameters_MustResultExpected()
      {
         var userID = System.Guid.NewGuid().ToString();
         var expirationDate = DateTime.UtcNow.AddMinutes(1);

         var value = RefreshToken.Create(userID, expirationDate);

         Assert.NotNull(value);
         Assert.NotEmpty(value.TokenID);
         Assert.Equal(36, value.TokenID.Length);
         Assert.Equal(userID, value.UserID);
         Assert.True(DateTime.UtcNow < value.ExpirationDate);
      }

      [Theory]
      [MemberData(nameof(Create_WithInvalidParameters_MustThrowException_Data))]
      public void Create_WithInvalidParameters_MustThrowException(string exceptionText, string userID, DateTime expirationDate)
      {
         var exception = Assert.Throws<ArgumentException>(() => RefreshToken.Create(userID, expirationDate));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Create_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { RefreshToken.WARNINGS.INVALID_USERID_PARAMETER, (string)null, DateTime.UtcNow.AddMinutes(1) },
            new object[] { RefreshToken.WARNINGS.INVALID_USERID_PARAMETER, "", DateTime.UtcNow.AddMinutes(1) },
            new object[] { RefreshToken.WARNINGS.INVALID_USERID_PARAMETER, " ", DateTime.UtcNow.AddMinutes(1) },
            new object[] { RefreshToken.WARNINGS.INVALID_USERID_PARAMETER, "123456789_123456789_123456789_12345", DateTime.UtcNow.AddMinutes(1) },
            new object[] { RefreshToken.WARNINGS.INVALID_USERID_PARAMETER, "123456789_123456789_123456789_1234567", DateTime.UtcNow.AddMinutes(1) },
            new object[] { RefreshToken.WARNINGS.INVALID_EXPIRATIONDATE_PARAMETER, "123456789_123456789_123456789_123456", null },
            new object[] { RefreshToken.WARNINGS.INVALID_EXPIRATIONDATE_PARAMETER, "123456789_123456789_123456789_123456", DateTime.MinValue }
         };

   }
}