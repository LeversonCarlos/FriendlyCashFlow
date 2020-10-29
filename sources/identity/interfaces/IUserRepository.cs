using System;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   public interface IUserRepository
   {

      Task<bool> GetUserByUserNameAsync(string userName);
      Task<IUserEntity> GetUserByUserIDAsync(Guid userID);

      Task AddUserAsync(IUserEntity user);

   }
}