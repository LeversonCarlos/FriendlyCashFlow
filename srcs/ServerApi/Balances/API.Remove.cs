using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Balances
{

   partial class BalancesService
   {

      internal async Task RemoveAsync(Entries.EntryData value)
      {
         try
         {
            if (!value.AccountID.HasValue || value.AccountID == 0) { return; }
            var date = new DateTime(value.SearchDate.Year, value.SearchDate.Month, 1);
            var user = this.GetService<Helpers.User>();

            // TRY TO LOCATE DATA
            var data = await this.GetDataQuery()
               .Where(x => x.Date == date && x.AccountID == value.AccountID.Value)
               .FirstOrDefaultAsync();
            if (data == null) { return; }

            // APPLY VALUE
            var entryValue = value.EntryValue * (value.Type == (short)Categories.enCategoryType.Expense ? -1 : 1);
            data.TotalValue -= entryValue;
            if (value.Paid) { data.PaidValue -= entryValue; }
            await this.dbContext.SaveChangesAsync();

         }
         catch (Exception) { throw; }
      }

   }

}
