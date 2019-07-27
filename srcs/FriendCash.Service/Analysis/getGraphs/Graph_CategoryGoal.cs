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

      #region GetGraphs_CategoryGoal
      private bool GetGraphs_CategoryGoal(Model.graphMainTemp resultData)
      {
         try
         {

            // INITIALIZE
            resultData.categoryGoal = new Model.graphCategoryGoal();

            // GROUP DATA
            var groupsQuery =
               from D in resultData.data.Daily
               join C in resultData.data.Dimensions.Categories on D.idCategory equals C.ID
               where D.Type == (short)Categories.Model.enCategoryType.Expense
               group D by new { C.ParentText, D.Paid } into G
               select new { ParentText = G.Key.ParentText, Paid = G.Key.Paid, Value = G.Sum(s => s.Value) };
            var groups = groupsQuery.ToList();

            // SERIES DATA
            var seriesData = groups
               .Select(x => new
               {
                  Name = x.ParentText,
                  TotalValue = x.Value,
                  PaidValue = (x.Paid == 1 ? x.Value : (double)0),
                  UnpaidValue = (x.Paid == 0 ? x.Value : (double)0)
               })
               .GroupBy(x => x.Name)
               .Select(x => new
               {
                  Name = x.Key,
                  colorIndex = resultData.mainCategories.Where(c => c.Key == x.Key).Select(c => c.Value).FirstOrDefault(),
                  GoalValue = resultData.categoryTrend.seriesData.Where(s => s.name == x.Key).SelectMany(s => s.data).Average(),
                  TotalValue = x.Sum(s => s.TotalValue),
                  PaidValue = x.Sum(s => s.PaidValue),
                  UnpaidValue = x.Sum(s => s.UnpaidValue)
               })
               .ToList();

            // MAX
            if (seriesData.Count != 0) {
               resultData.categoryGoal.maxValue = seriesData
                  .Select(x => (short)(x.TotalValue / x.GoalValue * 100))
                  .Max();
            }
            if (resultData.categoryGoal.maxValue < 105) { resultData.categoryGoal.maxValue = 105; }


            // CATEGORY LIST
            resultData.categoryGoal.categoryList = seriesData
               .OrderBy(x => x.Name)
               .Select(x => x.Name)
               .ToList();

            // PAID DATA
            resultData.categoryGoal.paidData = new Model.graphCategoryGoalData()
            {
               name = this.GetTranslation("LABEL_ANALYSIS_DIMENSION_PAID"),
               data = seriesData
                  .OrderBy(x => x.Name)
                  .Select(x => new Model.graphCategoryGoalDataValue()
                  {
                     colorIndex = x.colorIndex,
                     colorBrightness = 0,
                     y = (short)(x.PaidValue / x.GoalValue * 100),
                     realValue = Math.Round(x.PaidValue, 2),
                     goalValue = Math.Round(x.GoalValue, 0)
                  })
                  .ToList()
            };

            // UNPAID DATA
            resultData.categoryGoal.unpaidData = new Model.graphCategoryGoalData()
            {
               name = this.GetTranslation("LABEL_ANALYSIS_DIMENSION_UNPAID"),
               data = seriesData
                  .OrderBy(x => x.Name)
                  .Select(x => new Model.graphCategoryGoalDataValue()
                  {
                     colorIndex = x.colorIndex,
                     colorBrightness = 0.2,
                     y = (short)(x.UnpaidValue / x.GoalValue * 100),
                     realValue = Math.Round(x.UnpaidValue, 2)
                  })
                  .ToList()
            };

            // RESULT
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
