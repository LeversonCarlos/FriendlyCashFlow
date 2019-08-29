using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Entries
{

   partial class EntriesService
   {

      public async Task<ActionResult<EntryVM>> CreateAsync(EntryVM value)
      {
         try
         {

            // VALIDATE
            /*
            var validateMessage = await this.ValidateDataAsync(value);
            var validateResult = this.GetValue(validateMessage);
            if (!validateResult) { return validateMessage.Result; }
            */

            // NEW MODEL
            var user = this.GetService<Helpers.User>();
            var data = new EntryData()
            {
               ResourceID = user.ResourceID,
               Type = (short)value.Type,
               Text = value.Text,
               CategoryID = value.CategoryID,
               DueDate = value.DueDate,
               Value = Math.Abs(value.Value),
               Paid = value.Paid,
               RowStatus = 1
            };
            if (value.Paid && value.PayDate.HasValue) { data.PayDate = value.PayDate; }
            if (value.AccountID.HasValue) { data.AccountID = value.AccountID; }

            // TRANSFER
            // if (!string.IsNullOrEmpty(value.idTransfer))
            // { oData.idTransfer = value.idTransfer; }

            // PATTERN
            var patternService = this.GetService<Patterns.PatternsService>();
            data.PatternID = await patternService.AddPatternAsync(data.CategoryID, data.Text);

            // RECURRENCY
            // var newRecurrency = false; Model.bindRecurrency recurrencyModel = null;
            // if (value.idRecurrency.HasValue && value.idRecurrency.Value != 0) { oData.idRecurrency = value.idRecurrency.Value; }
            // else
            // {
            //    recurrencyModel = await this.addRecurrency(value);
            //    if (recurrencyModel != null) { oData.idRecurrency = recurrencyModel.idRecurrency; newRecurrency = true; }
            // }

            // SEARCH DATE
            data.SearchDate = data.DueDate;
            if (data.Paid && data.PayDate.HasValue) { data.SearchDate = data.PayDate.Value; }

            // APPLY
            await this.dbContext.Entries.AddAsync(data);
            await this.dbContext.SaveChangesAsync();

            // SORTING
            this.ApplySorting(data);
            await this.dbContext.SaveChangesAsync();

            // BALANCE
            var balanceService = this.GetService<Balances.BalancesService>();
            await balanceService.AddBalanceAsync(data);

            // RESULT
            var result = EntryVM.Convert(data);
            return this.CreatedResponse("entries", result.EntryID, result);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class EntriesController
   {
      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<EntryVM>> CreateAsync([FromBody]EntryVM value)
      {
         using (var service = new EntriesService(this.serviceProvider))
         { return await service.CreateAsync(value); }
      }
   }

}
