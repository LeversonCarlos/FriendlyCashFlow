using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Patterns
{

   partial class PatternsService
   {

      internal async Task RemovePatternAsync(Entries.EntryVM value)
      {
         try
         {

            // TRY TO LOCATE PATTERN
            if (value.PatternID <= 0) { return; }
            var data = await this.GetDataQuery()
               .Where(x => x.PatternID == value.PatternID)
               .FirstOrDefaultAsync();
            if (data == null) { return; }

            // DECREASE QUANTITY AND SAVE IT
            data.Count--;
            if (data.Count < 0) { data.Count = 0; }

            // DELETE REMOVED ROWS
            if (data.Count == 0)
            {
               data.RowStatus = -1;
               this.dbContext.Remove(data);
            }

            // APPLY
            await this.dbContext.SaveChangesAsync();

         }
         catch (Exception) { throw; }
      }

   }

}
