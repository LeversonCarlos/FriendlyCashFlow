namespace Lewio.CashFlow.Domain.Accounts.Services;

partial class SaveService
{

   private async Task<bool> SaveData()
   {
      try
      {

         IAccount? data;
         if (_Request.Data.IsNew())
         {
            data = await _MainRepository.Accounts.GetNew();
            // data.UserID = currentUserID;
            data.AccountID = Guid.NewGuid();
            _Request.Data.AccountID = data.AccountID;
         }
         else
            data = await _MainRepository.Accounts.GetByID(_Request.Data.AccountID!.Value);

         data.Apply(_Request.Data);

         if (!await _MainRepository.Accounts.Save(data))
            return SetWarningAndReturn("Accounts_SaveService_SaveData_Error");

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
