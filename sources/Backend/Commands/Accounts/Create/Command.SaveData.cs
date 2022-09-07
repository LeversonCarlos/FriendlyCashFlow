namespace Lewio.CashFlow.Accounts;

partial class CreateCommand
{

   private async Task<bool> SaveData()
   {
      try
      {

         _Request.Account.AccountID = EntityID.New();

         var entity = _Request.Account
            .ToEntity();

         entity.UserID = _ServiceProvider
            .GetRequiredService<Users.LoggedInUser>();

         await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .SaveNew(entity);

         _Response.Account = entity
            .ToModel();

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
