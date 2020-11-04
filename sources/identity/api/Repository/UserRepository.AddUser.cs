using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public Task AddUserAsync(IUserEntity value) =>
         _Collection.InsertOneAsync(value);

   }
}