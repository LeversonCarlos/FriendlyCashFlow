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

      [Theory]
      [MemberData(nameof(Constructor_WithInvalidParameters_MustThrowException_Data))]
      public void Constructor_WithInvalidParameters_MustThrowException(string exceptionText, Shared.EntityID accountID, string accountText, enAccountType accountType, short? closingDay, short? dueDay)
      {
         var active = true;
         var exception = Assert.Throws<ArgumentException>(() => new AccountEntity(accountID, accountText, accountType, closingDay, dueDay, active));

         Assert.NotNull(exception);
         Assert.Equal(exceptionText, exception.Message);
      }
      public static IEnumerable<object[]> Constructor_WithInvalidParameters_MustThrowException_Data() =>
         new[] {
            new object[] { AccountEntity.WARNING_INVALID_ACCOUNTID, null, "accountText", enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, new Shared.EntityID(), (string)null, enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, new Shared.EntityID(), "", enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, new Shared.EntityID(), " ", enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, new Shared.EntityID(), new string('0', 101), enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_CLOSING_DAY, new Shared.EntityID(), "accountText", enAccountType.CreditCard, (short)0, null},
            new object[] { AccountEntity.WARNING_INVALID_CLOSING_DAY, new Shared.EntityID(), "accountText", enAccountType.CreditCard, (short)32, null},
            new object[] { AccountEntity.WARNING_INVALID_DUE_DAY, new Shared.EntityID(), "accountText", enAccountType.CreditCard, (short)1, (short)0 },
            new object[] { AccountEntity.WARNING_INVALID_DUE_DAY, new Shared.EntityID(), "accountText", enAccountType.CreditCard, (short)1, (short)32 }
         };

   }
}
