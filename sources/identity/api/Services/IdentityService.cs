using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoDatabase _MongoDatabase;
      readonly ValidatePasswordSettings _Settings;

      public IdentityService(IMongoDatabase mongoDatabase, ValidatePasswordSettings settings)
      {
         _MongoDatabase = mongoDatabase;
         _Settings = settings;
      }

      internal Task<IMongoCollection<IUser>> GetCollectionAsync() =>
         _MongoDatabase.GetCollectionAsync<IUser>("users");

   }

   public partial interface IIdentityService
   {
   }

}
