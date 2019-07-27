#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
#endregion

namespace FriendCash.Service.Analysis
{
   partial class AnalysyController
   {

      #region GetGraphs_CategoryTrend
      private bool GetGraphs_CategoryTrend(Model.graphMainTemp resultData)
      {
         try
         {

            // INITIALIZE
            resultData.categoryTrend = new Model.graphCategoryTrend();

            // GROUP DATA
            var groupsQuery =
               from M in resultData.data.Monthly
               join C in resultData.data.Dimensions.Categories on M.idCategory equals C.ID
               where M.Type == (short)Categories.Model.enCategoryType.Expense
               group M by new { C.ParentText, M.SearchDate } into G
               select new { ParentText = G.Key.ParentText, SearchDate = G.Key.SearchDate, Value = G.Sum(s => s.Value) };
            var groups = groupsQuery.OrderBy(x => x.ParentText).ToList();

            // INTERVAL
            var currentDate = resultData.data.currentMonth.InitialDate;
            var minDate = currentDate.AddMonths(-11);
            var maxDate = currentDate.AddMonths(2).AddDays(-1);
            /*
            var minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var maxDate = minDate.AddMonths(1).AddDays(-1);
            var minDateText = groups.Select(x => x.SearchDate).FirstOrDefault();
            if (!string.IsNullOrEmpty(minDateText)) {
               minDate = new DateTime(int.Parse(minDateText.Substring(0, 4)), int.Parse(minDateText.Substring(4, 2)), 1);
            }
            var maxDateText = groups.Select(x => x.SearchDate).LastOrDefault();
            if (!string.IsNullOrEmpty(maxDateText)) {
               maxDate = new DateTime(int.Parse(maxDateText.Substring(0, 4)), int.Parse(maxDateText.Substring(4, 2)), 1);
            }
            */

            // CATEGORIES
            var categoryTemp = new Dictionary<string, string>();           
            currentDate = minDate;
            while (currentDate <= maxDate)
            {
               categoryTemp.Add(currentDate.ToString("yyyyMM"), currentDate.ToString("MMMMM"));
               currentDate = currentDate.AddMonths(1);
            }
            resultData.categoryTrend.categoryList = categoryTemp.OrderBy(x => int.Parse(x.Key)).Select(x => x.Value).ToList();

            // SERIES
            resultData.categoryTrend.seriesData = groups
               .GroupBy(x => x.ParentText)
               .Select(x => new Model.graphCategoryTrendData()
               {
                  name = x.Key,
                  colorIndex = resultData.mainCategories.Where(c => c.Key == x.Key).Select(c => c.Value).FirstOrDefault()
               })
               .ToList();

            // LOOP THROUGH SERIES
            foreach (var serie in resultData.categoryTrend.seriesData)
            {
               var values = groups.Where(x => x.ParentText == serie.name).ToList();
               var dataQuery =
                  from C in categoryTemp
                  join joinV in values on C.Key equals joinV.SearchDate into intoV
                  from V in intoV.DefaultIfEmpty()
                  select new { SearchDate = int.Parse(C.Key), Value = (V == null ? (double)0 : Math.Round(V.Value, 2)) };
               var data = dataQuery.OrderBy(x => x.SearchDate).ToList();
               serie.data = data.Select(x => x.Value).ToList();
            }

            // LIMITS
            resultData.categoryTrend.limits = new Model.graphLimits();
            if (resultData.categoryTrend.seriesData.Count != 0) {
               resultData.categoryTrend.limits.minValue = resultData.categoryTrend.seriesData.SelectMany(x => x.data).Min();
               resultData.categoryTrend.limits.maxValue = resultData.categoryTrend.seriesData.SelectMany(x => x.data).Max();
            }
            var difValue = resultData.categoryTrend.limits.maxValue - resultData.categoryTrend.limits.minValue;
            var stepsValue = Math.Ceiling(difValue / 4); // ROUNDING UP
            resultData.categoryTrend.limits.tickInterval = Math.Ceiling(stepsValue / 500) * 500;

            // RESULT
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
