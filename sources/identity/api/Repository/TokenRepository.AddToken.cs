using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class TokenRepository
   {

      public Task AddTokenAsync(IRefreshToken value) =>
         _Collection.InsertOneAsync(value);

   }
}