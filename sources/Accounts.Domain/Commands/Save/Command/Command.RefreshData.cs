namespace Lewio.CashFlow.Accounts;

partial class SaveCommand
{

   private async Task<bool> RefreshData()
   {
      try
      {

         var accountID = _Request.Data.AccountID!.Value;

         _Response.Data = await _AccountRepository
            .GetByID(accountID)
            .As<AccountEntity>();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
