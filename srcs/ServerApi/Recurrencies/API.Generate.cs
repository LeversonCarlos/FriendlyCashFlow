using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<bool> GenerateRecurrencyAsync(long recurrencyID)
      {
         try
         {
            var entriesService = this.GetService<Entries.EntriesService>();

            // RECURRENCY
            var recurrency = await this.GetDataQuery()
               .Where(x => x.RecurrencyID == recurrencyID)
               .FirstOrDefaultAsync();
            if (recurrency == null) { return false; }

            // PATTERN
            var patternMessage = await this.GetService<Patterns.PatternsService>().GetPatternAsync(recurrency.PatternID);
            var pattern = this.GetValue(patternMessage);
            if (pattern == null) { return false; }

            // DEFINE QUANTITY TO GENERATE
            var totalQuantity = recurrency.Count;
            if (totalQuantity == 0) { totalQuantity = 3; }

            // LOOP
            var generatedQuantity = 1;
            while (generatedQuantity < totalQuantity)
            {
               // FREQUENCY
               recurrency.EntryDate = this.GenerateRecurrencyAsync_NextDate((enRecurrencyType)recurrency.Type, recurrency.EntryDate);

               // MODEL
               var entryVM = new Entries.EntryVM
               {
                  Text = pattern.Text,
                  Type = pattern.Type,
                  CategoryID = pattern.CategoryID,
                  DueDate = recurrency.EntryDate,
                  EntryValue = Math.Abs(recurrency.EntryValue),
                  AccountID = recurrency.AccountID,
                  PatternID = recurrency.PatternID,
                  RecurrencyID = recurrency.RecurrencyID,
                  Paid = false
               };
               var entryMessage = await entriesService.CreateAsync(entryVM);
               var entryResult = this.GetValue(entryMessage);

               // NEXT
               generatedQuantity++;
            }

            // REMOVE COMPLETED RECURRENCY
            if (recurrency.Count != 0)
            {
               recurrency.RowStatus = -1;
               await this.dbContext.SaveChangesAsync();
            }

            // RESULT
            return true;
         }
         catch (Exception) { throw; }
      }

      private DateTime GenerateRecurrencyAsync_NextDate(enRecurrencyType type, DateTime entryDate)
      {
         if (type == enRecurrencyType.Weekly) { return entryDate.AddDays(7); }
         else if (type == enRecurrencyType.Monthly) { return entryDate.AddMonths(1); }
         else if (type == enRecurrencyType.Bimonthly) { return entryDate.AddMonths(2); }
         else if (type == enRecurrencyType.Quarterly) { return entryDate.AddMonths(3); }
         else if (type == enRecurrencyType.SemiYearly) { return entryDate.AddMonths(6); }
         else if (type == enRecurrencyType.Yearly) { return entryDate.AddYears(1); }
         else { return entryDate; }
      }

   }

}
