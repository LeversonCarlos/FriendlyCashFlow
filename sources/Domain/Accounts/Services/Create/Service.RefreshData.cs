namespace Lewio.CashFlow.Domain.Accounts.Services;

partial class CreateService
{

   private async Task<bool> RefreshData()
   {
      try
      {

         var account = await _MainRepository.Accounts.GetByID(_Response.Data!.AccountID);

         if (account == null)
            return SetWarningAndReturn("Accounts_NotFound_Error");

         _Response.Data = account.To<AccountEntity>();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
