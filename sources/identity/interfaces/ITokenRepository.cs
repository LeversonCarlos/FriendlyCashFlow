using System.Threading.Tasks;

namespace Elesse.Identity
{
   public interface ITokenRepository
   {

      Task<IRefreshToken> RetrieveRefreshTokenAsync(string refreshToken);

      Task AddRefreshTokenAsync(IRefreshToken value);

   }
}