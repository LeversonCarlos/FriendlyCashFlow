using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Transfers
{
   partial class TransferRepository
   {

      public Task UpdateAsync(ITransferEntity value) =>
         _Collection
            .ReplaceOneAsync(entity => entity.TransferID == value.TransferID, value as TransferEntity);

   }
}
