using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public Task AddUserAsync(IUserEntity user) =>
         _Collection.InsertOneAsync(user);

   }
}