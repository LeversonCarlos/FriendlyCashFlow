using MongoDB.Driver;

namespace Elesse.Patterns
{
   internal partial class PatternRepository : IPatternRepository
   {

      public PatternRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-patterns";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<PatternEntity> _Collection =>
         _MongoDatabase.GetCollection<PatternEntity>(_CollectionName);

   }
}
