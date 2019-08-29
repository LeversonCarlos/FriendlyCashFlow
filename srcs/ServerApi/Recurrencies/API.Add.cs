using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<long?> AddRecurrencyAsync(Recurrencies.RecurrencyVM value)
      {
         try
         {
            var user = this.GetService<Helpers.User>();

            // VALIDATION
            if (value == null) { return null; }
            if (value.RecurrencyID != 0) { return value.RecurrencyID; }

            // ADD NEW
            var data = new RecurrencyData
            {
               ResourceID = user.ResourceID,
               PatternID = value.PatternID,
               InitialDate = value.EntryDate,
               EntryDate = value.EntryDate,
               EntryValue = value.EntryValue,
               Type = (short)value.Type,
               Count = value.Count,
               RowStatus = 1
            };
            await this.dbContext.Recurrencies.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // RESULT
            return data.RecurrencyID;
         }
         catch (Exception) { throw; }
      }

   }

}
