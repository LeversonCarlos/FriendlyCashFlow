using System.Threading.Tasks;
using MongoDB.Driver;

namespace Elesse.Accounts
{
   partial class AccountRepository
   {

      public async Task<IAccountEntity[]> SearchAccountsAsync(string searchText)
      {
         // var rowStatusFilter = Builders<AccountEntity>.Filter.Eq(x => x.RowStatus, true);
         // var textFilter = Builders<AccountEntity>.Filter.Regex(x => x.Text, searchText);
         // var filter = Builders<AccountEntity>.Filter.And(rowStatusFilter, textFilter);
         var list = await _Collection
            //.Find(filter)
            .Find(entity => entity.RowStatus == true && entity.Text.ToLower().Contains(searchText.ToLower()))
            .ToListAsync();
         return list.ToArray();
      }

   }
}
