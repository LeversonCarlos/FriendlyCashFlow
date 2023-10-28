using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<List<YearlyBudgetVM>>> GetYearlyBudgetAsync(short searchYear, short searchMonth)
      {
         try
         {
            var YearlyBudget = await this.GetYearlyBudgetAsync_Execute(searchYear, searchMonth);
            if (YearlyBudget == null) { return this.WarningResponse("data query error"); }
            return YearlyBudget;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<List<YearlyBudgetVM>> GetYearlyBudgetAsync_Execute(short searchYear, short searchMonth)
      {
         try
         {
            var queryPath = "FriendlyCashFlow.ServerApi.Analytics.QUERY.YearlyBudget.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);

               if (!await queryReader.ExecuteReaderAsync()) { return null; }
               var entriesList = await queryReader.GetDataResultAsync<YearlyBudgetVM>();
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

      [HttpGet("yearlyBudget/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<List<YearlyBudgetVM>>> GetYearlyBudgetAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetYearlyBudgetAsync(searchYear, searchMonth);
      }

   }

   public class YearlyBudgetVM
   {
      public long CategoryID { get; set; }
      public string CategoryText { get; set; }

      public decimal? BudgetValue { get; set; }

      public decimal? MonthValue { get; set; }
      public decimal MonthPercentage { get; set; }
      public decimal? YearValue { get; set; }
      public decimal YearPercentage { get; set; }
   }

}
