using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Transfers
{
   partial class TransferRepository
   {

      public async Task<ITransferEntity> LoadAsync(Shared.EntityID transferID) =>
         await _Collection
            .Find(entity => entity.TransferID == transferID)
            .SingleOrDefaultAsync();

   }
}
