using MongoDB.Driver;

namespace Elesse.Entries
{
   internal partial class EntryRepository : IEntryRepository
   {

      public EntryRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-entries";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<EntryEntity> _Collection =>
         _MongoDatabase.GetCollection<EntryEntity>(_CollectionName);

   }
}
