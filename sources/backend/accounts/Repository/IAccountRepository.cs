using System.Threading.Tasks;

namespace Elesse.Accounts
{
   internal interface IAccountRepository
   {

      Task InsertAccountAsync(IAccountEntity value);
      Task UpdateAccountAsync(IAccountEntity value);
      Task DeleteAccountAsync(Shared.EntityID accountID);

      Task<IAccountEntity[]> ListAccountsAsync();
      Task<IAccountEntity> LoadAccountAsync(Shared.EntityID accountID);
      Task<IAccountEntity[]> SearchAccountsAsync(string searchText);

   }
}
