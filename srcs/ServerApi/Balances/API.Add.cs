using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Balances
{

   partial class BalancesService
   {

      internal async Task AddAsync(Entries.EntryData value)
      {
         var startTime = DateTime.Now;
         try
         {
            if (!value.AccountID.HasValue || value.AccountID == 0) { return; }
            var date = new DateTime(value.SearchDate.Year, value.SearchDate.Month, 1);

            // TRY TO LOCATE DATA
            var data = await this.GetDataQuery()
               .Where(x => x.Date == date && x.AccountID == value.AccountID.Value)
               .FirstOrDefaultAsync();

            // ADD NEW IF DOESNT FOUND
            if (data == null)
            {
               data = new BalanceData
               {
                  ResourceID = this.GetService<Helpers.User>().ResourceID,
                  Date = date,
                  AccountID = value.AccountID.Value,
                  RowStatus = 1
               };
               await this.dbContext.Balances.AddAsync(data);
            }

            // APPLY VALUE
            var entryValue = value.EntryValue * (value.Type == (short)Categories.enCategoryType.Expense ? -1 : 1);
            data.TotalValue += entryValue;
            if (value.Paid)
            {
               data.PaidValue += entryValue;
               if (date.Year != value.DueDate.Year || date.Month != value.DueDate.Month)
               {
                  data.TotalValue -= entryValue;
                  this.TrackEvent("Add Balance in diff Month", $"AccountID:{value.AccountID}",
                     $"SearchDate:{value.SearchDate.ToString("yyyy-MM")}", $"DueDate:{value.DueDate.ToString("yyyy-MM")}",
                     $"EntryValue:{entryValue}", $"Paid:{value.Paid}",
                     $"TotalValue:{data.TotalValue}", $"PaidValue:{data.PaidValue}");
               }
            }
            await this.dbContext.SaveChangesAsync();

         }
         catch (Exception) { throw; }
         finally { this.TrackMetric("Add Balance", Math.Round(DateTime.Now.Subtract(startTime).TotalMilliseconds, 0)); }
      }

   }

}
