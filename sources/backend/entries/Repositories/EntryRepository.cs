using System.Threading.Tasks;
using Elesse.Shared;
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

      public Task<IEntryEntity[]> ListAsync(int year, int month) =>
         throw new System.NotImplementedException();

      public Task<IEntryEntity> LoadAsync(EntityID entryID) =>
         throw new System.NotImplementedException();

      public Task InsertAsync(IEntryEntity value) =>
         throw new System.NotImplementedException();

      public Task UpdateAsync(IEntryEntity value) =>
         throw new System.NotImplementedException();

      public Task DeleteAsync(EntityID entryID) =>
         throw new System.NotImplementedException();

   }
}
