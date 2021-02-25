using System.Threading.Tasks;

namespace Elesse.Balances
{
   internal interface IBalanceRepository
   {

      Task InsertAsync(IBalanceEntity value);
      Task UpdateAsync(IBalanceEntity value);
      Task DeleteAsync(Shared.EntityID id);

      Task<IBalanceEntity[]> ListAsync(int year, int month);
      Task<IBalanceEntity> LoadAsync(Shared.EntityID id);

   }
}
