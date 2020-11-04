using System.Threading.Tasks;

namespace Elesse.Identity
{
   public interface IUserRepository
   {

      Task<IUserEntity> GetUserByUserIDAsync(string userID);
      Task<IUserEntity> GetUserByUserNameAsync(string userName);

      Task AddUserAsync(IUserEntity value);

   }
}