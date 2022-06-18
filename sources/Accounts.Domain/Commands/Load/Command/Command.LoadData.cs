namespace Lewio.CashFlow.Accounts;

partial class LoadCommand
{

   private async Task<bool> LoadData()
   {
      try
      {

         _Response.Data = await _AccountRepository
            .GetByID(_Request.AccountID)
            .As<AccountEntity>();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
