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
                  if (!item.Value.HasValue && item.Childs != null)
                  {
                     item.Value = item.Childs.Select(x => x.Value).Sum();
                     item.AverageValue = item.Childs.Select(x => x.AverageValue).Sum();
                  }
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
      public decimal? Value { get; set; }
      public decimal? AverageValue { get; set; }
      public decimal? Percent { get; set; }
      public List<CategoryGoalsVM> Childs { get; set; }
   }

}
