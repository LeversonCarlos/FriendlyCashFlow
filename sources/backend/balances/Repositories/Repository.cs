using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Balances
{
   internal partial class BalanceRepository : IBalanceRepository
   {

      public BalanceRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-balances";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<BalanceEntity> _Collection =>
         _MongoDatabase.GetCollection<BalanceEntity>(_CollectionName);

      public Task InsertAsync(IBalanceEntity value) => null;
      public Task UpdateAsync(IBalanceEntity value) => null;

   }
}
