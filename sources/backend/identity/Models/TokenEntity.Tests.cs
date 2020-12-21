using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Identity.Tests
{
   public class TokenEntityTests
   {

      [Fact]
      public void Create_WithValidParameters_MustResultExpected()
      {
         var userID = System.Guid.NewGuid().ToString();
         var expirationDate = DateTime.UtcNow.AddMinutes(1);

         var value = TokenEntity.Create(userID, expirationDate);

         Assert.NotNull(value);
         Assert.NotEmpty(value.TokenID);
         Assert.Equal(userID, value.UserID);
         Assert.True(DateTime.UtcNow < value.ExpirationDate);
      }

      [Theory]
      [MemberData(nameof(Create_WithInvalidParameters_MustThrowException_Data))]
      public void Create_WithInvalidParameters_MustThrowException(string exceptionText, string userID, DateTime expirationDate)
      {
         var exception = Assert.Throws<ArgumentException>(() => TokenEntity.Create(userID, expirationDate));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Create_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { TokenEntity.WARNINGS.INVALID_USERID_PARAMETER, (string)null, DateTime.UtcNow.AddMinutes(1) },
            new object[] { TokenEntity.WARNINGS.INVALID_USERID_PARAMETER, "", DateTime.UtcNow.AddMinutes(1) },
            new object[] { TokenEntity.WARNINGS.INVALID_USERID_PARAMETER, " ", DateTime.UtcNow.AddMinutes(1) },
            new object[] { TokenEntity.WARNINGS.INVALID_USERID_PARAMETER, "123456789_123456789_123456789_12345", DateTime.UtcNow.AddMinutes(1) },
            new object[] { TokenEntity.WARNINGS.INVALID_USERID_PARAMETER, "123456789_123456789_123456789_1234567", DateTime.UtcNow.AddMinutes(1) },
            new object[] { TokenEntity.WARNINGS.INVALID_EXPIRATIONDATE_PARAMETER, "123456789_123456789_123456789_123456", null },
            new object[] { TokenEntity.WARNINGS.INVALID_EXPIRATIONDATE_PARAMETER, "123456789_123456789_123456789_123456", DateTime.MinValue }
         };

   }
}