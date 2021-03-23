using MongoDB.Driver;

namespace Elesse.Recurrences
{
   internal partial class RecurrenceRepository : IRecurrenceRepository
   {

      public RecurrenceRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-recurrences";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<RecurrenceEntity> _Collection =>
         _MongoDatabase.GetCollection<RecurrenceEntity>(_CollectionName);

   }
}
