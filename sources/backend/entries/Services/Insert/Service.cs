using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Entries
{
   partial class EntryService
   {

      public async Task<IActionResult> InsertAsync(InsertVM insertVM)
      {
         try
         {

            if (insertVM == null)
               return Warning(WARNINGS.INVALID_INSERT_PARAMETER);

            if (insertVM.Recurrence == null)
               await InsertNewEntryAsync(insertVM, null);
            else
               await InsertNewRecurrencesAsync(insertVM);

            _InsightsService.TrackEvent("Entry Service Insert");
            return Ok();
         }
         catch (ArgumentException argEx) { return Warning(argEx.Message); }
         catch (Exception ex) { return Shared.Results.Exception(ex); }
      }

      private async Task<Shared.EntityID> InsertNewEntryAsync(InsertVM insertVM, EntryRecurrenceEntity entryRecurrence)
      {
         var pattern = await _PatternService.IncreaseAsync(insertVM.Pattern);
         var entry = EntryEntity.Create(pattern, insertVM.AccountID, insertVM.DueDate, insertVM.Value);
         if (insertVM.Paid && insertVM.PayDate.HasValue)
            entry.SetPayment(insertVM.PayDate.Value, insertVM.Value);
         if (entryRecurrence != null)
            entry.SetRecurrence(entryRecurrence);
         entry.RefreshSorting();
         await _EntryRepository.InsertAsync(entry);
         await IncreaseBalanceAsync(entry);
         return entry.EntryID;
      }

      private async Task<Shared.EntityID[]> InsertNewRecurrencesAsync(InsertVM insertVM)
      {
         var entityIDs = new List<Shared.EntityID>();
         var pattern = await _PatternService.RetrieveAsync(insertVM.Pattern);
         var recurrenceProperties = Recurrences.RecurrenceProperties.Create(pattern.PatternID, insertVM.AccountID, insertVM.DueDate, insertVM.Value, insertVM.Recurrence.Type);
         var recurrenceID = await _RecurrenceService.GetNewRecurrenceAsync(recurrenceProperties);

         for (short currentOccurrence = 1; currentOccurrence <= insertVM.Recurrence.TotalOccurrences; currentOccurrence++)
         {
            var entryRecurrence = EntryRecurrenceEntity.Restore(recurrenceID, currentOccurrence, insertVM.Recurrence.TotalOccurrences);
            var entityID = await InsertNewEntryAsync(insertVM, entryRecurrence);
            entityIDs.Add(entityID);
            insertVM.DueDate = insertVM.DueDate.AddMonths(1);
         }

         return entityIDs.ToArray();
      }

      private Task<Balances.IBalanceEntity> IncreaseBalanceAsync(EntryEntity entry)
      {
         var accountID = entry.AccountID;
         var date = new DateTime(entry.SearchDate.Year, entry.SearchDate.Month, 1, 12, 0, 0);
         var value = entry.Value * (entry.Pattern.Type == Patterns.enPatternType.Income ? 1 : -1);
         var paid = entry.Paid;
         return _BalanceService.IncreaseAsync(accountID, date, value, paid);
      }

   }
}
