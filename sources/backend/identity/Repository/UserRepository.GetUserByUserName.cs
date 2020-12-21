using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public async Task<IUserEntity> GetUserByUserNameAsync(string userName) =>
         await _Collection
            .Find(user => user.UserName == userName)
            .SingleOrDefaultAsync();

   }
}