using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public async Task<IUserEntity> GetUserByUserIDAsync(string userID) =>
         await _Collection
            .Find(user => user.UserID == userID)
            .SingleOrDefaultAsync();

   }
}