namespace Lewio.CashFlow.Accounts;

partial class RemoveCommand
{

   private async Task<bool> RemoveData()
   {
      try
      {

         var data = await _AccountRepository.GetByID(_Request.Data.AccountID!.Value);

         if (!await _AccountRepository.Remove(data))
            return SetWarningAndReturn("Accounts_RemoveCommand_RemoveData_Error");

         return true;
      }
      catch (Exception ex) { return SetErrorAndReturn(ex); }
   }

}
