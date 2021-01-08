using System.Threading.Tasks;

namespace Elesse.Accounts
{
   public interface IAccountRepository
   {

      Task InsertAccountAsync(IAccountEntity value);
      Task UpdateAccountAsync(IAccountEntity value);
      Task DeleteAccountAsync(Shared.EntityID accountID);

      Task<IAccountEntity[]> ListAccountsAsync();
      Task<IAccountEntity> GetAccountByIDAsync(Shared.EntityID accountID);
      Task<IAccountEntity[]> SearchAccountsAsync(string searchText);

   }
}
