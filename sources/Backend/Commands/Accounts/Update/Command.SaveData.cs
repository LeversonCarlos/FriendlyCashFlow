namespace Lewio.CashFlow.Accounts;

partial class UpdateCommand
{

   private async Task<bool> SaveData()
   {
      try
      {

         var entity = await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .GetByID(_Request.Account.AccountID);

         if (entity == null)
            return SetWarningAndReturn("Account not found");

         entity
            .Apply(_Request.Account);

         await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .Save(entity);

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
