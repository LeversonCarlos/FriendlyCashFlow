using Microsoft.EntityFrameworkCore;

namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   internal Task<AccountEntity[]> GetListBySearchTerms(string searchTerms);
}

partial class AccountRepository
{

   async Task<AccountEntity[]> IAccountRepository.GetListBySearchTerms(string searchTerms)
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
