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
            var previousMonthBalanceMessage = await this.GetService<Dashboard.DashboardService>().GetBalanceAsync((short)previousMonth.Year, (short)previousMonth.Month, false);
            var currentMonthBalance = await this.GetService<Dashboard.DashboardService>().GetBalanceAsync(searchYear, searchMonth, false);
            var currentrMonthFlow = await this.GetService<Entries.EntriesService>().GetDataAsync(searchYear, searchMonth, 0);

            var previousMonthBalance =
               PatrimonyResumeItem.Create(
                  PatrimonyResumeEnum.PreviousMonthBalance,
                  previousMonthBalanceMessage.Value
                     .Select(x => x.CurrentBalance)
                     .Sum(),
                  this.GetTranslation);

            var currentMonthEntryValues = currentrMonthFlow.Value
               .SelectMany(x => x.EntryList)
               .Where(x => x.EntryID > 0 && string.IsNullOrEmpty(x.TransferID))
               .Select(x => new
               {
                  IncomeValue = x.EntryValue > 0 ? x.EntryValue : 0,
                  ExpenseValue = x.EntryValue < 0 ? x.EntryValue : 0
               })
               .ToList();

            var currentMonthIncome =
               PatrimonyResumeItem.Create(
                  PatrimonyResumeEnum.CurrentMonthIncome,
                  currentMonthEntryValues
                     .Select(x => x.IncomeValue)
                     .Sum(),
                  this.GetTranslation);
            var currentMonthExpense =
               PatrimonyResumeItem.Create(
                  PatrimonyResumeEnum.CurrentMonthExpense,
                  currentMonthEntryValues
                     .Select(x => x.ExpenseValue)
                     .Sum(),
                  this.GetTranslation);

            var patrimonyResume = new PatrimonyResumeItem[]{
               previousMonthBalance,
               currentMonthIncome,
               currentMonthExpense,
               PatrimonyResumeItem.Create(
                  PatrimonyResumeEnum.CurrentMonthBalance,
                  previousMonthBalance.Value+currentMonthIncome.Value+currentMonthExpense.Value,
               this.GetTranslation
               )
            };

            var patrimonyAccountDistribution = currentMonthBalance.Value
               .Where(x => x.CurrentBalance > 0)
               .Select(x => new { x.AccountID, x.CurrentBalance })
               .ToArray();
            var patrimonyAccountDetails = this
               .GetService<Accounts.AccountsService>()
               .GetDataQuery()
               .Where(x => patrimonyAccountDistribution.Select(i => i.AccountID).ToArray().Contains(x.AccountID))
               .Select(x => new { x.AccountID, x.Text })
               .ToArray();
            var patrimonyDistribution = patrimonyAccountDistribution
               .OrderByDescending(x => x.CurrentBalance)
               .Select(x => new PatrimonyDistributionItem
               {
                  Text = patrimonyAccountDetails
                     .Where(i => i.AccountID == x.AccountID)
                     .Select(i => i.Text)
                     .FirstOrDefault(),
                  Value = x.CurrentBalance
               })
               .ToArray();

            var patrimony = PatrimonyVM.Create(patrimonyResume, patrimonyDistribution);
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
      public PatrimonyResumeItem[] PatrimonyResume { get; private set; }
      public PatrimonyDistributionItem[] PatrimonyDistribution { get; private set; }
      public static PatrimonyVM Create(
         PatrimonyResumeItem[] patrimonyResume,
         PatrimonyDistributionItem[] patrimonyDistribution) =>
         new PatrimonyVM
         {
            PatrimonyResume = patrimonyResume,
            PatrimonyDistribution = patrimonyDistribution
         };
   }

   public struct PatrimonyDistributionItem
   {
      public string Text { get; set; }
      public decimal Value { get; set; }
   }

   public enum PatrimonyResumeEnum : short { PreviousMonthBalance = 0, CurrentMonthIncome = 1, CurrentMonthExpense = 2, CurrentMonthBalance = 3 };
   public struct PatrimonyResumeItem
   {
      public PatrimonyResumeEnum Type { get; private set; }
      public decimal Value { get; private set; }
      public string Text { get; private set; }
      public static PatrimonyResumeItem Create(
         PatrimonyResumeEnum type, decimal value,
         Func<string, string> getTranslation) =>
         new PatrimonyResumeItem
         {
            Type = type,
            Value = value,
            Text = getTranslation($"{"Analytics".ToUpper()}_{nameof(PatrimonyResumeEnum).ToUpper()}_{type.ToString().ToUpper()}")
         };
   }

}
