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
      var query = _DataContext.Accounts
         .WithLoggedInUser(GetLoggedInUser())
         .WithSearchTerms(searchTerms, (entity, term) => entity.Text.Contains(term));

      var dataList = await query.ToArrayAsync();

      return dataList;
   }

}
