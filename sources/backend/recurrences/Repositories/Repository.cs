using System.Threading.Tasks;
using Elesse.Shared;
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

      public Task InsertAsync(IRecurrenceEntity value) =>
         throw new System.NotImplementedException();
      public Task UpdateAsync(IRecurrenceEntity value) =>
         throw new System.NotImplementedException();
      public Task<IRecurrenceEntity> LoadAsync(EntityID recurrenceID) =>
         throw new System.NotImplementedException();

   }
}
