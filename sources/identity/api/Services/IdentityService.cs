using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoDatabase _MongoDatabase;
      public IdentityService(IMongoDatabase mongoDatabase)
      {
         _MongoDatabase = mongoDatabase;
      }

      internal Task<IMongoCollection<IUser>> GetCollectionAsync() =>
         _MongoDatabase.GetCollectionAsync<IUser>("users");

   }

   public partial interface IIdentityService
   {
   }

}
