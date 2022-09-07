namespace Lewio.CashFlow.Accounts;

partial class LoadCommand
{

   private async Task<bool> LoadData()
   {
      try
      {

         if (!_Request.AccountID.IsValid())
            return SetWarningAndReturn("Invalid accountID parameter");

         var data = await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .GetByID(_Request.AccountID);

         if (data == null)
            return SetWarningAndReturn("Account not found");

         _Response.Account = data
            ?.ToModel();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
