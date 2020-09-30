using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoDatabase _MongoDatabase;
      internal const string CollectionName = "users";
      readonly PasswordSettings _Settings;

      public IdentityService(IMongoDatabase mongoDatabase, PasswordSettings settings)
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
