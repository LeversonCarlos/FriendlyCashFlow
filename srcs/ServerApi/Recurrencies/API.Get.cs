using System.Linq;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      private IQueryable<RecurrencyData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Recurrencies
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

   }

}
