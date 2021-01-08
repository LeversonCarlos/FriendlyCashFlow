using System.Threading.Tasks;
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

      public Task<IAccountEntity[]> SearchAccountsAsync(string searchText) =>
         throw new System.NotImplementedException();

   }
}
