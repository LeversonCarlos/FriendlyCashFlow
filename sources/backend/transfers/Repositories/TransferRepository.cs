using MongoDB.Driver;

namespace Elesse.Transfers
{
   internal partial class TransferRepository : ITransferRepository
   {

      public TransferRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-transfers";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<TransferEntity> _Collection =>
         _MongoDatabase.GetCollection<TransferEntity>(_CollectionName);

   }
}
