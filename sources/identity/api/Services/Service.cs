using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      const string _CollectionName = "users";
      readonly IMongoDatabase _MongoDatabase;
      readonly IdentitySettings _Settings;

      public IdentityService(IMongoDatabase mongoDatabase, IdentitySettings settings)
      {
         _MongoDatabase = mongoDatabase;
         _Settings = settings;
      }

      internal IMongoCollection<IUser> _Collection =>
         _MongoDatabase.GetCollection<IUser>(_CollectionName);

      internal Task<IMongoCollection<IUser>> GetCollectionAsync() =>
         _MongoDatabase.GetCollectionAsync<IUser>("users");

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
