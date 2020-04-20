using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<List<CategoryGoalsVM>>> GetCategoryGoalsAsync(short searchYear, short searchMonth)
      {
         try
         {
            var categoryList = await this.GetCategoryGoalsAsync_Execute(searchYear, searchMonth);
            if (categoryList == null) { return this.WarningResponse("data query error"); }

            var categoryGoals = this.GetCategoryGoalsAsync_Merge(categoryList);
            return categoryGoals;

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<List<CategoryGoalsVM>> GetCategoryGoalsAsync_Execute(short searchYear, short searchMonth)
      {
         try
         {
            var queryPath = "FriendlyCashFlow.ServerApi.Analytics.QUERY.CategoryGoals.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);

               if (!await queryReader.ExecuteReaderAsync()) { return null; }
               return await queryReader.GetDataResultAsync<CategoryGoalsVM>();
            }
         }
         catch (Exception) { throw; }
      }

      private List<CategoryGoalsVM> GetCategoryGoalsAsync_Merge(List<CategoryGoalsVM> categoryList)
      {
         try
         {

            Func<long, List<CategoryGoalsVM>> GetChilds = null;
            GetChilds = new Func<long, List<CategoryGoalsVM>>(parentID =>
            {
               var result = categoryList.Where(x => x.ParentID == parentID).ToList();

               foreach (var item in result)
               {
                  item.Childs = GetChilds(item.CategoryID);
                  if (!item.CategoryValue.HasValue && item.Childs != null)
                  {
                     item.CategoryValue = item.Childs.Select(x => x.CategoryValue).Sum();
                     item.AverageValue = item.Childs.Select(x => x.AverageValue).Sum();
                  }
               }

               var maxAverage = result?.Where(x => x.AverageValue.HasValue).Select(x => x.AverageValue.Value).DefaultIfEmpty().Max() ?? 0;
               foreach (var item in result)
               {
                  if (item.CategoryValue.HasValue && item.AverageValue.HasValue && item.AverageValue.Value > 0 && maxAverage > 0)
                  {
                     var categoryPercent = item.CategoryValue.Value / item.AverageValue.Value * 100;
                     var weight = item.AverageValue.Value / maxAverage;
                     categoryPercent = (categoryPercent - 100) * weight;
                     item.CategoryPercent = Math.Round(100 + categoryPercent, 1);
                  }
                  else { item.CategoryPercent = 0; }
               }

               return result;
            });

            return GetChilds(0);
         }
         catch (Exception) { throw; }
      }

   }

   partial class AnalyticsController
   {

      [HttpGet("categoryGoals/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<List<CategoryGoalsVM>>> GetCategoryGoalsAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetCategoryGoalsAsync(searchYear, searchMonth);
      }

   }

   public class CategoryGoalsVM
   {
      public long CategoryID { get; set; }
      public string Text { get; set; }
      public long ParentID { get; set; }
      public decimal? CategoryValue { get; set; }
      public decimal? CategoryPercent { get; set; }
      public decimal? AverageValue { get; set; }
      public List<CategoryGoalsVM> Childs { get; set; }
   }

}
