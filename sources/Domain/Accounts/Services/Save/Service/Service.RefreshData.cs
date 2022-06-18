namespace Lewio.CashFlow.Domain.Accounts.Services;

partial class SaveService
{

   private async Task<bool> RefreshData()
   {
      try
      {

         var accountID = _Request.Data.AccountID!.Value;

         _Response.Data = await _MainRepository.Accounts
            .GetByID(accountID)
            .As<AccountEntity>();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
