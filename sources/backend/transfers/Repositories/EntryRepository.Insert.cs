using System.Threading.Tasks;

namespace Elesse.Transfers
{
   partial class TransferRepository
   {

      public Task InsertAsync(ITransferEntity value) =>
         _Collection
            .InsertOneAsync(value as TransferEntity);

   }
}
