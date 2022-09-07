using Lewio.CashFlow.Users;

namespace Lewio.CashFlow.Common;

partial class IQueryableExtensions
{

   public static IQueryable<T> WithLoggedInUser<T>(this IQueryable<T> query, LoggedInUser? loggedInUser)
      where T : BaseEntity
   {

      if (loggedInUser == null)
         query = query.Where(x => x.UserID == "");

      else
         query = query.Where(x => x.UserID == loggedInUser);

      return query;
   }

}
