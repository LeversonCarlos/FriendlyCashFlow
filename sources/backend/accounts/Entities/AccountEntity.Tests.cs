using System;
using System.Collections.Generic;
using Xunit;

namespace Elesse.Accounts.Tests
{
   public class AccountEntityTests
   {


      [Theory]
      [InlineData("Bank Text", enAccountType.Bank, null, null)]
      [InlineData("General Text", enAccountType.General, null, null)]
      [InlineData("Investment Text", enAccountType.Investment, null, null)]
      [InlineData("Service Text", enAccountType.Service, null, null)]
      [InlineData("Credit Card Text", enAccountType.CreditCard, (short)7, (short)15)]
      public void Constructor_WithValidParameters_MustResultValidInstance(string accountText, enAccountType accountType, short? closingDay, short? dueDay)
      {
         var account = new AccountEntity(accountText, accountType, closingDay, dueDay);

         Assert.NotNull(account);
         Assert.NotNull(account.AccountID);
         Assert.Equal(36, ((string)account.AccountID).Length);
         Assert.Equal(accountText, account.Text);
         Assert.Equal(accountType, account.Type);
         Assert.Equal(closingDay, account.ClosingDay);
         Assert.Equal(dueDay, account.DueDay);
         Assert.True(account.Active);
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
            new object[] { AccountEntity.WARNING_INVALID_TEXT, Shared.EntityID.NewID(), (string)null, enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, Shared.EntityID.NewID(), "", enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, Shared.EntityID.NewID(), " ", enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_TEXT, Shared.EntityID.NewID(), new string('0', 101), enAccountType.General, null, null},
            new object[] { AccountEntity.WARNING_INVALID_CLOSING_DAY, Shared.EntityID.NewID(), "accountText", enAccountType.CreditCard, (short)0, null},
            new object[] { AccountEntity.WARNING_INVALID_CLOSING_DAY, Shared.EntityID.NewID(), "accountText", enAccountType.CreditCard, (short)32, null},
            new object[] { AccountEntity.WARNING_INVALID_DUE_DAY, Shared.EntityID.NewID(), "accountText", enAccountType.CreditCard, (short)1, (short)0 },
            new object[] { AccountEntity.WARNING_INVALID_DUE_DAY, Shared.EntityID.NewID(), "accountText", enAccountType.CreditCard, (short)1, (short)32 }
         };

   }
}
