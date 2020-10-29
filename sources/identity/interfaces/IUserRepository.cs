using System;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   public interface IUserRepository
   {

      Task<IUserEntity> GetUserByUserIDAsync(Guid userID);
      Task<IUserEntity> GetUserByUserNameAsync(string userName);

      Task AddUserAsync(IUserEntity user);

   }
}