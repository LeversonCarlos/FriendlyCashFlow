using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public async Task AddUserAsync(IUserEntity value) =>
         await _Collection
            .InsertOneAsync(value as UserEntity);

   }
}