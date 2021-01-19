using MongoDB.Driver;

namespace Elesse.Categories
{
   internal partial class CategoryRepository : ICategoryRepository
   {

      public CategoryRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-categories";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<CategoryEntity> _Collection =>
         _MongoDatabase.GetCollection<CategoryEntity>(_CollectionName);

   }
}
