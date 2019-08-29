using System.Linq;

namespace FriendlyCashFlow.API.Patterns
{

   partial class PatternsService
   {

      private IQueryable<PatternData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Patterns
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

   }

}
