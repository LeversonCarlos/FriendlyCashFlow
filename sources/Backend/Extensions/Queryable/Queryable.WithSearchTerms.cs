namespace Lewio.CashFlow.Common;

partial class IQueryableExtensions
{

   public static IQueryable<T> WithSearchTerms<T>(this IQueryable<T> query, string? searchTerms, Func<T, string, bool> predicate)
      where T : BaseEntity
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
