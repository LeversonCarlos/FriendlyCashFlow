namespace Lewio.CashFlow.Domain.Accounts.Services;

partial class CreateService
{

   private async Task<bool> SaveData()
   {
      try
      {

         _Response.Data = new AccountEntity
         {
            AccountID = System.Guid.NewGuid(),
            Type = _Request.Type,
            Text = "New Account",
            IsActive = true
         };

         if (_Request.Type == AccountTypeEnum.CreditCard)
         {
            _Response.Data.CreditCardClosingDay = 10;
            _Response.Data.CreditCardDueDay = 15;
         }

         if (!await _MainRepository.Accounts.SaveNew(_Response.Data))
            return SetWarningAndReturn("Accounts_SaveData_Error");

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
