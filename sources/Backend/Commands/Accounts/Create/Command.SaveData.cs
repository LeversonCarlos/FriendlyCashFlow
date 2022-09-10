namespace Lewio.CashFlow.Accounts;

partial class CreateCommand
{

   private async Task<bool> SaveData()
   {
      try
      {

         _Response.AccountID = EntityID.New();
         _Request.Account.AccountID = _Response.AccountID;

         var entity = _Request.Account
            .ToEntity();

         entity.UserID = _ServiceProvider
            .GetRequiredService<Users.LoggedInUser>();

         entity.RowStatus = 1;

         await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .SaveNew(entity);

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
