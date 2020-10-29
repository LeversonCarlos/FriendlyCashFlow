using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public Task<IUserEntity> GetUserByUserNameAsync(string userName) =>
         _Collection.Find<IUserEntity>(user => user.UserName == userName).SingleAsync();

   }
}