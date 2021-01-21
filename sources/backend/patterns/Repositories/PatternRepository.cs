using System.Threading.Tasks;
using Elesse.Shared;
using MongoDB.Driver;

namespace Elesse.Patterns
{
   internal partial class PatternRepository : IPatternRepository
   {

      public PatternRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-data";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<PatternEntity> _Collection =>
         _MongoDatabase.GetCollection<PatternEntity>(_CollectionName);

      public Task<IPatternEntity> LoadPatternAsync(EntityID patternID) =>
         throw new System.NotImplementedException();

      public Task<IPatternEntity> LoadPatternAsync(enPatternType type, EntityID categoryID, string text) =>
         throw new System.NotImplementedException();

      public Task InsertAsync(IPatternEntity value) =>
         throw new System.NotImplementedException();

      public Task UpdateAsync(IPatternEntity value) =>
         throw new System.NotImplementedException();

      public Task DeleteAsync(EntityID patternID) =>
         throw new System.NotImplementedException();

   }
}