using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class TokenRepository
   {

      public async Task<ITokenEntity> RetrieveRefreshTokenAsync(string refreshToken) =>
         await _Collection
            .FindOneAndDeleteAsync(x => x.TokenID == refreshToken);

   }
}