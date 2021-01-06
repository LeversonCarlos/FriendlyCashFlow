using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Accounts.Tests
{
   public class AccountEntityTests
   {


      [Theory]
      [InlineData("Bank Text", enAccountType.Bank)]
      [InlineData("General Text", enAccountType.General)]
      [InlineData("Investment Text", enAccountType.Investment)]
      [InlineData("Service Text", enAccountType.Service)]
      public void SimpleConstructor_WithValidParameters_MustResultValidInstance(string accountText, enAccountType accountType)
      {
         var account = new AccountEntity(accountText, accountType);

         Assert.NotNull(account);
         Assert.NotNull(account.AccountID);
         Assert.Equal(36, account.AccountID.ToString().Length);
         Assert.Equal(accountText, account.Text);
         Assert.Equal(accountType, account.Type);
         Assert.True(account.Active);
         Assert.Null(account.ClosingDay);
         Assert.Null(account.DueDay);
      }

      [Fact]
      public void CreditCardConstructor_WithValidParameters_MustResultValidInstance()
      {
         var accountText = "Credit Card Text";
         short closingDay = 7;
         short dueDay = 15;
         var account = new AccountEntity(accountText, closingDay, dueDay);

         Assert.NotNull(account);
         Assert.NotNull(account.AccountID);
         Assert.Equal(36, account.AccountID.ToString().Length);
         Assert.Equal(accountText, account.Text);
         Assert.Equal(enAccountType.CreditCard, account.Type);
         Assert.True(account.Active);
         Assert.Equal(closingDay, account.ClosingDay);
         Assert.Equal(dueDay, account.DueDay);
      }

      /*
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

      */

   }
}
