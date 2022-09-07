namespace Lewio.CashFlow.Accounts;

partial class LoadCommand
{

   private async Task<bool> LoadData()
   {
      try
      {

         var data = await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .GetByID(_Request.AccountID);

         _Response.Account = data
            ?.ToModel();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
