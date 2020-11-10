using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class TokenRepository
   {

      public async Task AddRefreshTokenAsync(ITokenEntity value) =>
         await _Collection
            .InsertOneAsync(value as RefreshToken);

   }
}