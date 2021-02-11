using System.Threading.Tasks;

namespace Elesse.Transfers
{
   internal interface ITransferRepository
   {

      Task InsertAsync(ITransferEntity value);
      Task UpdateAsync(ITransferEntity value);
      Task DeleteAsync(Shared.EntityID id);

      Task<ITransferEntity[]> ListAsync(int year, int month);
      Task<ITransferEntity> LoadAsync(Shared.EntityID id);

   }
}
