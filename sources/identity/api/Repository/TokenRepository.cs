using MongoDB.Driver;

namespace Elesse.Identity
{
   internal partial class TokenRepository : ITokenRepository
   {

      public TokenRepository(IMongoDatabase mongoDatabase)
      {
         _MongoDatabase = mongoDatabase;
      }

      readonly IMongoDatabase _MongoDatabase;

      internal const string UserCollectionName = "user-tokens";
      IMongoCollection<RefreshToken> _Collection =>
         _MongoDatabase.GetCollection<RefreshToken>(UserCollectionName);

   }
}