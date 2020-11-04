using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public Task<IUserEntity> GetUserByUserIDAsync(string userID) =>
         _Collection.Find<IUserEntity>(user => user.UserID == userID).SingleAsync();

   }
}