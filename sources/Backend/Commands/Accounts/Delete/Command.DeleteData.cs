namespace Lewio.CashFlow.Accounts;

partial class DeleteCommand
{

   private async Task<bool> DeleteData()
   {
      try
      {

         var entity = await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .GetByID(_Request.AccountID);

         if (entity == null)
            return SetWarningAndReturn("Account not found");

         entity.RowStatus = 0;

         await _ServiceProvider
            .GetRequiredService<IAccountRepository>()
            .Save(entity);

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
