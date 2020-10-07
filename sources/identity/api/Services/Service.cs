using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoDatabase _MongoDatabase;
      internal const string CollectionName = "users";
      readonly IdentitySettings _Settings;

      public IdentityService(IMongoDatabase mongoDatabase, IdentitySettings settings)
      {
         _MongoDatabase = mongoDatabase;
         _Settings = settings;
      }

      internal Task<IMongoCollection<IUser>> GetCollectionAsync() =>
         _MongoDatabase.GetCollectionAsync<IUser>("users");

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
