using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<List<MonthlyBudgetVM>>> GetMonthlyBudgetAsync(short searchYear, short searchMonth)
      {
         try
         {
            var monthlyBudget = await this.GetMonthlyBudgetAsync_Execute(searchYear, searchMonth);
            if (monthlyBudget == null) { return this.WarningResponse("data query error"); }
            return monthlyBudget;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<List<MonthlyBudgetVM>> GetMonthlyBudgetAsync_Execute(short searchYear, short searchMonth)
      {
         try
         {
            var queryPath = "FriendlyCashFlow.ServerApi.Analytics.QUERY.MonthlyBudget.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);

               if (!await queryReader.ExecuteReaderAsync()) { return null; }
               var entriesList = await queryReader.GetDataResultAsync<MonthlyBudgetVM>();
               var categoriesList = await queryReader.GetDataResultAsync<EntriesCategoryVM>();

               Func<long, long> getParentCategoryID = null;
               getParentCategoryID = new Func<long, long>(categoryID =>
               {
                  var parentID = categoriesList
                     .Where(x => x.CategoryID == categoryID)
                     .Select(x => x.ParentID)
                     .FirstOrDefault();
                  if (parentID == 0) { return categoryID; }
                  return getParentCategoryID(parentID);
               });

               foreach (var entry in entriesList)
               { entry.CategoryID = getParentCategoryID(entry.CategoryID); }

               return entriesList;
            }
         }
         catch (Exception) { throw; }
      }

   }

   partial class AnalyticsController
   {

      [HttpGet("monthlyBudget/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<List<MonthlyBudgetVM>>> GetMonthlyBudgetAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetMonthlyBudgetAsync(searchYear, searchMonth);
      }

   }

   public class MonthlyBudgetVM
   {
      public long PatternID { get; set; }
      public string Text { get; set; }
      public long CategoryID { get; set; }
      public double OverflowValue { get; set; }
      public double OverflowPercent { get; set; }
   }

}
