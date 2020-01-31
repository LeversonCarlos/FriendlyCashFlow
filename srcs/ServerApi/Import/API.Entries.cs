using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{

   partial class ImportService
   {

      internal async Task<ActionResult<bool>> CreateEntriesAsync(ImportVM value)
      {
         try
         {
            var entriesService = this.GetService<Entries.EntriesService>();
            if (value.Entries == null || value.Entries.Count == 0) { return this.OkResponse(true); }
            value.Entries = value.Entries.OrderBy(x => x.Account).ThenBy(x => x.DueDate).ToList();

            // NUMBER OF ROWS TO LOG AT EACH 10 PERCENTE
            var eachNthRows = Math.Floor((double)(value.Entries.Count / 10));

            // LOOP THROUGH ENTRIES
            for (int i = 0; i < value.Entries.Count; i++)
            {
               var entry = value.Entries[i];

               if ((i % eachNthRows) == 0)
               {
                  this.TrackEvent("Import Data - Importing Entries", $"UserID:{value.UserID}", $"Percent:{i / eachNthRows * 10}%");
               }

               var createParam = new Entries.EntryVM
               {
                  Text = entry.Text,
                  Type = entry.Type,
                  AccountID = entry.AccountID,
                  CategoryID = entry.CategoryID,
                  DueDate = entry.DueDate,
                  EntryValue = Math.Round(Math.Abs(entry.Value), 2),
                  Paid = entry.Paid,
                  PayDate = entry.PayDate,
                  RecurrencyID = entry.RecurrencyID,
                  EntryID = 0
               };
               if (!string.IsNullOrEmpty(entry.Recurrency) && !createParam.RecurrencyID.HasValue)
               {
                  createParam.Recurrency = new Recurrencies.RecurrencyVM
                  {
                     Type = Recurrencies.enRecurrencyType.Monthly,
                     Count = 1
                  };
               }

               var createMessage = await entriesService.CreateAsync(createParam);
               var createResult = this.GetValue(createMessage);
               if (createResult == null) { return createMessage.Result; }

               if (createParam.Recurrency != null)
               {
                  value.Entries
                     .Where(x => x.Recurrency == entry.Recurrency && !x.RecurrencyID.HasValue)
                     .ToList()
                     .ForEach(x => x.RecurrencyID = createResult.RecurrencyID);
               }
            }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

}
