using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<PatrimonyVM>> GetPatrimonyAsync(short searchYear, short searchMonth)
      {
         try
         {

            var previousMonth = (new DateTime(searchYear, searchMonth, 1)).AddMonths(-1);
            var previousMonthBalance = await this.GetService<Dashboard.DashboardService>().GetBalanceAsync((short)previousMonth.Year, (short)previousMonth.Month, false);
            var currentMonthBalance = await this.GetService<Dashboard.DashboardService>().GetBalanceAsync(searchYear, searchMonth, false);
            var currentrMonthFlow = await this.GetService<Entries.EntriesService>().GetDataAsync(searchYear, searchMonth, 0);

            var previousMonthBalanceTotal = previousMonthBalance.Value
               .Select(x => x.CurrentBalance)
               .Sum();

            var currentMonthEntryValues = currentrMonthFlow.Value
               .SelectMany(x => x.EntryList)
               .Where(x => x.EntryID > 0 && string.IsNullOrEmpty(x.TransferID))
               .Select(x => new
               {
                  IncomeValue = x.EntryValue > 0 ? x.EntryValue : 0,
                  ExpenseValue = x.EntryValue < 0 ? x.EntryValue : 0
               })
               .ToList();

            var currentMonthIncome = currentMonthEntryValues
               .Select(x => x.IncomeValue)
               .Sum();
            var currentMonthExpense = currentMonthEntryValues
               .Select(x => x.ExpenseValue)
               .Sum();



            var patrimony = new PatrimonyVM
            {
               PreviousMonthBalance = previousMonthBalanceTotal,
               CurrentMonthIncome = currentMonthIncome,
               CurrentMonthExpense = currentMonthExpense,
               PreviousMonth = previousMonthBalance.Value,
               CurrentMonth = currentMonthBalance.Value
            };

            return patrimony;

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class AnalyticsController
   {

      [HttpGet("patrimony/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<PatrimonyVM>> GetPatrimonyAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetPatrimonyAsync(searchYear, searchMonth);
      }

   }

   public class PatrimonyVM
   {
      public decimal PreviousMonthBalance { get; set; }
      public decimal CurrentMonthIncome { get; set; }
      public decimal CurrentMonthExpense { get; set; }
      public List<Dashboard.BalanceVM> PreviousMonth { get; set; }
      public List<Dashboard.BalanceVM> CurrentMonth { get; set; }
   }

}
