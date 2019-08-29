using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Balances
{

   partial class BalancesService
   {

      internal async Task AddBalanceAsync(Entries.EntryData value)
      {
         try
         {
            var user = this.GetService<Helpers.User>();
            if (!value.AccountID.HasValue || value.AccountID == 0) { return; }
            var date = new DateTime(value.SearchDate.Year, value.SearchDate.Month, 1);

            // TRY TO LOCATE DATA
            var data = await this.dbContext.Balances
               .Where(x => x.RowStatus == 1 && x.ResourceID == user.ResourceID)
               .Where(x => x.Date == date && x.AccountID == value.AccountID.Value)
               .FirstOrDefaultAsync();

            // ADD NEW IF DOESNT FOUND
            if (data == null)
            {
               data = new Balance
               {
                  ResourceID = user.ResourceID,
                  Date = date,
                  AccountID = value.AccountID.Value,
                  RowStatus = 1
               };
               await this.dbContext.Balances.AddAsync(data);
            }

            // APPLY VALUE
            var entryValue = value.Value * (value.Type == (short)Categories.enCategoryType.Expense ? -1 : 1);
            data.TotalValue += entryValue;
            if (value.Paid) { data.PaidValue += entryValue; }
            await this.dbContext.SaveChangesAsync();

         }
         catch (Exception) { throw; }
      }

   }

}
