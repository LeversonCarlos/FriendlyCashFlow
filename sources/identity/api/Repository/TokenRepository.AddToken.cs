using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class TokenRepository
   {

      public Task AddRefreshTokenAsync(IRefreshToken value) =>
         _Collection.InsertOneAsync(value);

   }
}