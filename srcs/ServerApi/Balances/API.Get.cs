using System.Linq;

namespace FriendlyCashFlow.API.Balances
{

   partial class BalancesService
   {

      private IQueryable<BalanceData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Balances
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

   }

}
