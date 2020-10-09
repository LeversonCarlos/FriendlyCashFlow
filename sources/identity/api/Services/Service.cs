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

      internal static string GetUserCollectionName() =>
         nameof(IUser).Substring(1).ToLower();
      internal IMongoCollection<IUser> _Collection =>
         _MongoDatabase.GetCollection<IUser>(GetUserCollectionName());

      internal static string GetRefreshTokenCollectionName() =>
         nameof(IRefreshToken).Substring(1).ToLower();
      internal IMongoCollection<IRefreshToken> _RefreshTokenCollection =>
         _MongoDatabase.GetCollection<IRefreshToken>(GetRefreshTokenCollectionName());

      internal Task<IMongoCollection<IUser>> GetCollectionAsync() =>
         _MongoDatabase.GetCollectionAsync<IUser>("users");

   }

   public partial interface IIdentityService { }

   internal partial struct WARNINGS { }

}
