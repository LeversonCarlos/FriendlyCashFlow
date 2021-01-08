using System.Threading.Tasks;
using Elesse.Shared;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   internal partial class AccountRepository : IAccountRepository
   {

      public AccountRepository(IMongoDatabase mongoDatabase)
      {
         _MongoDatabase = mongoDatabase;
      }

      readonly IMongoDatabase _MongoDatabase;

      internal const string AccountCollectionName = "accounts";
      IMongoCollection<AccountEntity> _Collection =>
         _MongoDatabase.GetCollection<AccountEntity>(AccountCollectionName);

      public Task InsertAccountAsync(IAccountEntity value) =>
         throw new System.NotImplementedException();
      public Task UpdateAccountAsync(IAccountEntity value) =>
         throw new System.NotImplementedException();
      public Task DeleteAccountAsync(Shared.EntityID accountID) =>
         throw new System.NotImplementedException();

      public Task<IAccountEntity[]> ListAccountsAsync() =>
         throw new System.NotImplementedException();
      public Task<IAccountEntity> GetAccountByIDAsync(EntityID accountID) =>
         throw new System.NotImplementedException();
      public Task<IAccountEntity[]> SearchAccountsAsync(string searchText) =>
         throw new System.NotImplementedException();

   }
}
