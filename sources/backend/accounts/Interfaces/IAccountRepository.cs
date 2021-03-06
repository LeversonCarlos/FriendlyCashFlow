using System.Threading.Tasks;

namespace Elesse.Accounts
{
   internal interface IAccountRepository
   {

      Task InsertAsync(IAccountEntity value);
      Task UpdateAsync(IAccountEntity value);
      Task DeleteAsync(Shared.EntityID accountID);

      Task<IAccountEntity[]> SearchAsync(string searchText);
      Task<IAccountEntity[]> ListAsync();
      Task<IAccountEntity> LoadAsync(Shared.EntityID accountID);

   }
}
