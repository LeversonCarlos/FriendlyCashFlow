using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

      internal async Task<long> GetPatternAsync(Entries.EntryVM value)
      {
         return await this.GetDataQuery()
            .Where(x => x.Type == (short)value.Type && x.CategoryID == value.CategoryID && x.Text == value.Text)
            .Select(x => x.PatternID)
            .FirstOrDefaultAsync();
      }

   }

}
