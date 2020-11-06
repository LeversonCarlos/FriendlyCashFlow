using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public async Task UpdateUserAsync(IUserEntity value) =>
         await _Collection
            .ReplaceOneAsync(x => x.UserID == value.UserID, value as User);

   }
}