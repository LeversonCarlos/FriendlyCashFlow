using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Transfers
{
   partial class TransferRepository
   {

      public Task DeleteAsync(Shared.EntityID transferID) =>
         _Collection
            .DeleteOneAsync(entity => entity.TransferID == transferID);

   }
}
