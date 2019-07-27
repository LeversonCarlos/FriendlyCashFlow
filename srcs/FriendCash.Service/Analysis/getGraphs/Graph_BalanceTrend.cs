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

      #region GetGraphs_BalanceTrend
      private bool GetGraphs_BalanceTrend(Model.graphMainTemp resultData)
      {
         try
         {

            // INITIALIZE
            resultData.balanceTrend = new Model.graphBalanceTrend();

            // GROUP DATA
            var groupsQuery =
               from M in resultData.data.Monthly
               group M by new { M.Type, M.SearchDate } into G
               select new { G.Key.Type, G.Key.SearchDate, Value = G.Sum(s => s.Value) };
            var groups = groupsQuery.ToList();

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
            resultData.balanceTrend.categoryList = categoryTemp.OrderBy(x => int.Parse(x.Key)).Select(x => x.Value).ToList();

            // INCOME SERIES
            resultData.balanceTrend.IncomeData = new Model.graphBalanceTrendData()
            {
               name = this.GetTranslation("LABEL_CATEGORIES_INCOME")
            };
            var incomeQuery =
               from C in categoryTemp
               join joinV in groups.Where(g => g.Type == (short)Categories.Model.enCategoryType.Income) on C.Key equals joinV.SearchDate into intoV
               from V in intoV.DefaultIfEmpty()
               select new { SearchDate = int.Parse(C.Key), Value = (V == null ? (double)0 : Math.Round(V.Value, 2) * +1) };
            resultData.balanceTrend.IncomeData.data = incomeQuery
               .OrderBy(x => x.SearchDate)
               .Select(x => x.Value)
               .ToList();

            // EXPENSE SERIES
            resultData.balanceTrend.ExpenseData = new Model.graphBalanceTrendData()
            {
               name = this.GetTranslation("LABEL_CATEGORIES_EXPENSE")
            };
            var expenseQuery =
               from C in categoryTemp
               join joinV in groups.Where(g => g.Type == (short)Categories.Model.enCategoryType.Expense) on C.Key equals joinV.SearchDate into intoV
               from V in intoV.DefaultIfEmpty()
               select new { SearchDate = int.Parse(C.Key), Value = (V == null ? (double)0 : Math.Round(V.Value, 2) * -1) };
            resultData.balanceTrend.ExpenseData.data = expenseQuery
               .OrderBy(x => x.SearchDate)
               .Select(x => x.Value)
               .ToList();

            // BALANCE SERIES
            resultData.balanceTrend.BalanceData = new Model.graphBalanceTrendData()
            {
               name = this.GetTranslation("TITLE_ANALYSIS_BALANCE_LABEL")
            };
            resultData.balanceTrend.BalanceData.data = new List<double>();
            for (int i = 0; i < resultData.balanceTrend.categoryList.Count; i++)
            {
               var incomeValue = resultData.balanceTrend.IncomeData.data[i];
               var expenseValue = resultData.balanceTrend.ExpenseData.data[i];
               var balanceValue = incomeValue - Math.Abs(expenseValue);
               resultData.balanceTrend.BalanceData.data.Add(balanceValue);
            }

            // LIMITS
            resultData.balanceTrend.limits = new Model.graphLimits();
            resultData.balanceTrend.limits.minValue = resultData.balanceTrend.ExpenseData.data.Min();
            resultData.balanceTrend.limits.maxValue = resultData.balanceTrend.IncomeData.data.Max();
            var difValue = resultData.balanceTrend.limits.maxValue - resultData.balanceTrend.limits.minValue;
            var stepsValue = Math.Ceiling(difValue / 4); // ROUNDING UP
            resultData.balanceTrend.limits.tickInterval = Math.Ceiling(stepsValue / 500) * 500;

            // RESULT
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
