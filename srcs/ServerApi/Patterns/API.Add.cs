using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Patterns
{

   partial class PatternsService
   {

      internal async Task<long?> AddAsync(Entries.EntryVM value)
      {
         var startTime = DateTime.Now;
         try
         {
            if (!value.CategoryID.HasValue) { return null; }

            // TRY TO LOCATE PATTERN
            var data = await this.GetDataQuery()
               .Where(x => x.Type == (short)value.Type && x.CategoryID == value.CategoryID.Value && x.Text == value.Text)
               .FirstOrDefaultAsync();

            // ADD NEW IF DOESNT FOUND
            if (data == null)
            {
               data = new PatternData
               {
                  ResourceID = this.GetService<Helpers.User>().ResourceID,
                  Type = (short)value.Type,
                  CategoryID = value.CategoryID.Value,
                  Text = value.Text,
                  Count = 0,
                  RowStatus = 1
               };
               await this.dbContext.Patterns.AddAsync(data);
            }

            // INCREASE QUANTITY AND SAVE IT
            data.Count++;
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return data.PatternID;
         }
         catch (Exception) { throw; }
         finally { this.TrackMetric("Add Pattern", Math.Round(DateTime.Now.Subtract(startTime).TotalMilliseconds, 0)); }
      }

   }

}
