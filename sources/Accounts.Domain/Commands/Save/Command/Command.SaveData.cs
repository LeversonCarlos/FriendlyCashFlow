namespace Lewio.CashFlow.Accounts;

partial class SaveCommand
{

   private async Task<bool> SaveData()
   {
      try
      {

         IAccountEntity? data;
         if (_Request.Data.IsNew())
         {
            data = await _AccountRepository.GetNew();
            // data.UserID = currentUserID;
            data.AccountID = Guid.NewGuid();
            _Request.Data.AccountID = data.AccountID;
         }
         else
            data = await _AccountRepository.GetByID(_Request.Data.AccountID!.Value);

         data.Apply(_Request.Data);

         if (!await _AccountRepository.Save(data))
            return SetWarningAndReturn("Accounts_SaveCommand_SaveData_Error");

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
