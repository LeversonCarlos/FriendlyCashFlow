using Microsoft.EntityFrameworkCore;
namespace Lewio.CashFlow.Accounts;

partial interface IAccountRepository
{
   Task<AccountEntity[]?> GetListBySearchTerms(string searchTerms);
}

partial class AccountRepository
{

   public async Task<AccountEntity[]?> GetListBySearchTerms(string searchTerms)
   {

      var searchTermsPredicate = (AccountEntity entity, string term) =>
      {
         return entity.Text.Contains(term);
      };

      var query = _DataContext.Accounts
         .WithLoggedInUser(GetLoggedInUser());
      // .WithSearchTerms(searchTerms, (entity, term) => entity.Text.Contains(term))

      var terms = searchTerms
         ?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
      if (terms?.Length > 0)
      {
         foreach (var term in terms)
            query = query
               .Where(entity => entity.Text.Contains(term));
      }

      var dataList = await query.ToArrayAsync();

      return dataList;
   }

}
