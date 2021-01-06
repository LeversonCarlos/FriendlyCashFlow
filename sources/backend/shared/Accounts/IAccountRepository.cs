using System.Threading.Tasks;

namespace Elesse.Accounts
{
   public interface IAccountRepository
   {

      Task AddAccountAsync(IAccountEntity value);
      Task UpdateAccountAsync(IAccountEntity value);

      Task<IAccountEntity[]> GetAccountsListAsync();
      Task<IAccountEntity> GetAccountByIDAsync(Shared.EntityID accountID);
      Task<IAccountEntity[]> SearchAccountsAsync(string searchText);

   }
}
