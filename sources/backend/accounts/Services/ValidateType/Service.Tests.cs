using Xunit;

namespace Elesse.Accounts.Tests
{
   partial class AccountServiceTests
   {

      [Theory]
      [InlineData(WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE, enAccountType.Bank, 1, 1)]
      [InlineData(WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE, enAccountType.Bank, null, 1)]
      [InlineData(WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE, enAccountType.Bank, 1, null)]
      [InlineData(WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE, enAccountType.General, 1, 1)]
      [InlineData(WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE, enAccountType.Investment, 1, 1)]
      [InlineData(WARNINGS.DAYS_ONLY_VALID_FOR_CREDIT_CARD_TYPE, enAccountType.Service, 1, 1)]
      [InlineData(WARNINGS.DAYS_REQUIRED_FOR_CREDIT_CARD_TYPE, enAccountType.CreditCard, null, null)]
      [InlineData(WARNINGS.DAYS_REQUIRED_FOR_CREDIT_CARD_TYPE, enAccountType.CreditCard, 1, null)]
      [InlineData(WARNINGS.DAYS_REQUIRED_FOR_CREDIT_CARD_TYPE, enAccountType.CreditCard, null, 1)]
      internal async void ValidateType_WithInvalidParameters_MustResultErrorMessage(string warningText, enAccountType type, short? closingDay, short? dueDay)
      {
         var service = new AccountService(null);
         var result = await service.ValidateTypeAsync(type, closingDay, dueDay);

         Assert.Equal(new string[] { warningText }, result);
      }

      [Theory]
      [InlineData(enAccountType.Bank, null, null)]
      [InlineData(enAccountType.General, null, null)]
      [InlineData(enAccountType.Investment, null, null)]
      [InlineData(enAccountType.Service, null, null)]
      [InlineData(enAccountType.CreditCard, 1, 1)]
      internal async void ValidateUsername_WithValidParameters_MustResultNoErrorMessages(enAccountType type, short? closingDay, short? dueDay)
      {
         var service = new AccountService(null);
         var result = await service.ValidateTypeAsync(type, closingDay, dueDay);

         Assert.Equal(new string[] { }, result);
      }

   }
}
