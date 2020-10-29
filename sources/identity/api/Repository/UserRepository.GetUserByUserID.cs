using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Elesse.Identity
{
   partial class UserRepository
   {

      public Task<IUserEntity> GetUserByUserIDAsync(Guid userID) =>
         _Collection.Find<IUserEntity>(user => user.UserID == userID.ToString()).SingleAsync();

   }
}