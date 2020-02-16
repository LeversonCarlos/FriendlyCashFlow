using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<List<MonthlyTargetVM>>> GetMonthlyTargetAsync(short searchYear, short searchMonth)
      {
         try
         {
            var monthlyTarget = await this.GetMonthlyTargetAsync_Execute(searchYear, searchMonth);
            if (monthlyTarget == null) { return this.WarningResponse("data query error"); }
            return monthlyTarget;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<List<MonthlyTargetVM>> GetMonthlyTargetAsync_Execute(short searchYear, short searchMonth)
      {
         try
         {
            var queryPath = "FriendlyCashFlow.ServerApi.Analytics.QUERY.MonthlyTarget.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);

               if (!await queryReader.ExecuteReaderAsync()) { return null; }
               return await queryReader.GetDataResultAsync<MonthlyTargetVM>();
            }
         }
         catch (Exception) { throw; }
      }

   }

   partial class AnalyticsController
   {

      [HttpGet("monthlyTarget/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<List<MonthlyTargetVM>>> GetMonthlyTargetAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetMonthlyTargetAsync(searchYear, searchMonth);
      }

   }

   public class MonthlyTargetVM
   {
      public DateTime SearchDate { get; set; }
      public string Text { get { return this.SearchDate.ToString("MMM yyyy"); } }
      public decimal IncomeValue { get; set; }
      public decimal IncomeAverage { get; set; }
      public decimal IncomeTarget { get; set; }
      public decimal ExpenseValue { get; set; }
      public decimal ExpenseAverage { get; set; }
      public decimal ExpenseTarget { get; set; }
   }

}
