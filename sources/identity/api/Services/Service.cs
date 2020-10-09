using FriendlyCashFlow.Identity.Helpers;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   internal partial class IdentityService : IIdentityService
   {

      readonly IMongoDatabase _MongoDatabase;
      readonly IdentitySettings _Settings;

      public IdentityService(IMongoDatabase mongoDatabase, IdentitySettings settings)
      {
         _MongoDatabase = mongoDatabase;
         _Settings = settings;
      }

      const string _UserCollectionName = "users";
      internal IMongoCollection<IUser> _Collection =>
         _MongoDatabase.GetCollection<IUser>(_UserCollectionName);

      const string _RefreshTokenCollectionName = "refresh-tokens";
      internal IMongoCollection<IRefreshToken> _RefreshTokenCollection =>
         _MongoDatabase.GetCollection<IRefreshToken>(_RefreshTokenCollectionName);

      internal Task<IMongoCollection<IUser>> GetCollectionAsync() =>
         _MongoDatabase.GetCollectionAsync<IUser>("users");

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
