using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<long?> CreateAsync(Recurrencies.RecurrencyVM value)
      {
         var startTime = DateTime.Now;
         try
         {

            // VALIDATION
            if (value == null) { return null; }
            if (value.RecurrencyID != 0) { return value.RecurrencyID; }

            // ADD NEW
            var data = new RecurrencyData
            {
               ResourceID = this.GetService<Helpers.User>().ResourceID,
               PatternID = value.PatternID,
               AccountID = value.AccountID,
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
         finally { this.TrackMetric("Create Recurrency", Math.Round(DateTime.Now.Subtract(startTime).TotalMilliseconds, 0)); }
      }

   }

}
