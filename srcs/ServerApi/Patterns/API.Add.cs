using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Patterns
{

   partial class PatternsService
   {

      internal async Task<long> AddPatternAsync(long categoryID, string entryText)
      {
         try
         {
            var user = this.GetService<Helpers.User>();

            // TRY TO LOCATE PATTERN
            var data = await this.dbContext.Patterns
               .Where(x => x.RowStatus == 1 && x.ResourceID == user.ResourceID)
               .Where(x => x.CategoryID == categoryID && x.Text == entryText)
               .FirstOrDefaultAsync();

            // ADD NEW IF DOESNT FOUND
            if (data == null)
            {
               data = new PatternData
               {
                  ResourceID = user.ResourceID,
                  CategoryID = categoryID,
                  Text = entryText,
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
      }

   }

}
