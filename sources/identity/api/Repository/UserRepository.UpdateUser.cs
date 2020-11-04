using MongoDB.Driver;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public Task UpdateUserAsync(IUserEntity value) =>
         _Collection.ReplaceOneAsync(x => x.UserID == value.UserID, value);

   }
}