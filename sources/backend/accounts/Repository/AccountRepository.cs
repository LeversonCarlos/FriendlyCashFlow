using System.Threading.Tasks;
using Elesse.Shared;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   internal partial class AccountRepository : IAccountRepository
   {

      public AccountRepository(IMongoDatabase mongoDatabase, Identity.IUser currentUser)
      {
         _MongoDatabase = mongoDatabase;
         _CollectionName = $"{currentUser.UserID}-accounts";
      }

      readonly IMongoDatabase _MongoDatabase;

      readonly string _CollectionName = null;
      IMongoCollection<AccountEntity> _Collection =>
         _MongoDatabase.GetCollection<AccountEntity>(_CollectionName);

      public Task InsertAccountAsync(IAccountEntity value) =>
         throw new System.NotImplementedException();
      public Task UpdateAccountAsync(IAccountEntity value) =>
         throw new System.NotImplementedException();
      public Task DeleteAccountAsync(Shared.EntityID accountID) =>
         throw new System.NotImplementedException();

      public Task<IAccountEntity> LoadAccountAsync(EntityID accountID) =>
         throw new System.NotImplementedException();
      public Task<IAccountEntity[]> SearchAccountsAsync(string searchText) =>
         throw new System.NotImplementedException();

   }
}
