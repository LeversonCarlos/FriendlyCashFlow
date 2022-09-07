namespace Lewio.CashFlow.Common;

public static class IQueryableExtensions
{

   public static IQueryable<T> WithSearchTerms<T>(this IQueryable<T> query, string? searchTerms, Func<T, string, bool> predicate)
   {

      var terms = searchTerms
         ?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

      if (terms?.Length > 0)
      {
         foreach (var term in terms)
            query = query
               .Where(x => predicate(x, term));
      }

      return query;
   }

}
