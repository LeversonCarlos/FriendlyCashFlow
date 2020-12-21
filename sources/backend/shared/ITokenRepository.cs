using System.Threading.Tasks;

namespace Elesse.Identity
{
   public interface ITokenRepository
   {

      Task<ITokenEntity> RetrieveRefreshTokenAsync(string refreshToken);

      Task AddRefreshTokenAsync(ITokenEntity value);

   }
}