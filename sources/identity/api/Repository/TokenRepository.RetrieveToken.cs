using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class TokenRepository
   {

      public Task<IRefreshToken> RetrieveRefreshTokenAsync(string refreshToken) =>
         _Collection.FindOneAndDeleteAsync(x => x.TokenID == refreshToken);

   }
}