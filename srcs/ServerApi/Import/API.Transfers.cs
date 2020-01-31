using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{

   partial class ImportService
   {

      internal async Task<ActionResult<bool>> CreateTransfersAsync(ImportVM value)
      {
         try
         {
            var transfersService = this.GetService<Transfers.TransfersService>();
            if (value.Transfers == null || value.Transfers.Count == 0) { return this.OkResponse(true); }
            value.Transfers = value.Transfers.OrderBy(x => x.IncomeAccount).ThenBy(x => x.Date).ToList();

            // NUMBER OF ROWS TO LOG AT EACH 10 PERCENTE
            var eachNthRows = Math.Floor((double)(value.Transfers.Count / 10));

            // LOOP THROUGH ENTRIES
            for (int i = 0; i < value.Transfers.Count; i++)
            {
               var transfer = value.Transfers[i];

               if ((i % eachNthRows) == 0)
               { this.TrackEvent("Import Data - Importing Transfers", $"UserID:{value.UserID}", $"Percent:{i / eachNthRows * 10}%"); }

               var createParam = new Transfers.TransferVM
               {
                  ExpenseAccountID = transfer.ExpenseAccountID.Value,
                  IncomeAccountID = transfer.IncomeAccountID.Value,
                  TransferDate = transfer.Date,
                  TransferValue = transfer.Value
               };

               var createMessage = await transfersService.CreateAsync(createParam);
               var createResult = this.GetValue(createMessage);
               if (!createResult) { return createMessage.Result; }

            }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

}
