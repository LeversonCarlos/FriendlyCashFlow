using Microsoft.EntityFrameworkCore;

namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   Task<AccountEntity[]> GetListBySearchTerms(string searchTerms);
}

partial class AccountRepository
{

   public async Task<AccountEntity[]> GetListBySearchTerms(string searchTerms)
   {

      var searchTermsPredicate = (AccountEntity entity, string term) =>
      {
         return entity.Text.Contains(term);
      };

      var query = _DataContext.Accounts
         .WithLoggedInUser(GetLoggedInUser())
         .WithSearchTerms(searchTerms, searchTermsPredicate);

      var dataList = await query.ToArrayAsync();

      return dataList;
   }

}
