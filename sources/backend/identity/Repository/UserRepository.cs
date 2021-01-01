using MongoDB.Driver;

namespace Elesse.Identity
{
   internal partial class UserRepository : IUserRepository
   {

      public UserRepository(IMongoDatabase mongoDatabase)
      {
         _MongoDatabase = mongoDatabase;
      }

      readonly IMongoDatabase _MongoDatabase;

      internal const string UserCollectionName = "users";
      IMongoCollection<UserEntity> _Collection =>
         _MongoDatabase.GetCollection<UserEntity>(UserCollectionName);

   }
}