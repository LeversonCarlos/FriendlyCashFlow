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

      #region GetGraphs_CategoryDetails

      private bool GetGraphs_CategoryDetails(Model.graphMainTemp resultData)
      {
         resultData.categoryExpenseDetails = this.GetGraphs_CategoryDetails(resultData, Categories.Model.enCategoryType.Expense);
         resultData.categoryIncomeDetails = this.GetGraphs_CategoryDetails(resultData, Categories.Model.enCategoryType.Income);
         return true;
      }

      private Model.graphCategoryDetails GetGraphs_CategoryDetails(Model.graphMainTemp resultData, Categories.Model.enCategoryType categoryType)
      {
         try
         {

            // GROUP DATA
            var groupsQuery =
               from D in resultData.data.Daily
               join C in resultData.data.Dimensions.Categories on D.idCategory equals C.ID
               join P in resultData.data.Dimensions.Patterns on D.idPattern equals P.ID
               join A in resultData.data.Dimensions.Accounts on D.idAccount equals A.ID
               where D.Type == (short)categoryType
               group D by new { Level1 = C.ParentText, Level2 = C.Text, Level3 = P.Text, Level4 = A.Text } into G
               select new { Level1 = G.Key.Level1, Level2 = G.Key.Level2, Level3 = G.Key.Level3, Level4 = G.Key.Level4, Value = G.Sum(s => s.Value) };
            var groups = groupsQuery.ToList();

            // DRILLDOWN
            var drillDown = groups
               .GroupBy(l1 => l1.Level1)
               .Select(l1 => new Model.graphCategoryDetailsData()
               {
                  name = l1.Key,
                  value = Math.Round(l1.Sum(s => s.Value), 2),
                  colorIndex = resultData.mainCategories.Where(c => c.Key == l1.Key).Select(c => c.Value).FirstOrDefault(),
                  drilldown = l1.Key.Replace(" \\ ", "").ToLower(),

                  drillData = groups
                     .Where(l2 => l2.Level1 == l1.Key)
                     .GroupBy(l2 => l2.Level2)
                     .Select(l2 => new Model.graphCategoryDetailsData()
                     {
                        id = l1.Key.Replace(" \\ ", "").ToLower(),
                        drilldown = l2.Key.Replace(" \\ ", "").ToLower(),
                        name = l2.Key.Replace(l1.Key + " \\ ", ""),
                        value = Math.Round(l2.Sum(s => s.Value), 2),

                        drillData = groups
                           .Where(l3 => l3.Level2 == l2.Key)
                           .GroupBy(l3 => l3.Level3)
                           .Select(l3 => new Model.graphCategoryDetailsData()
                           {
                              id = l2.Key.Replace(" \\ ", "").ToLower(),
                              drilldown = l3.Key.Replace(" \\ ", "").ToLower(),
                              name = l3.Key,
                              value = Math.Round(l3.Sum(s => s.Value), 2),

                              drillData = groups
                                 .Where(l4 => l4.Level3 == l3.Key)
                                 .GroupBy(l4 => l4.Level4)
                                 .Select(l4 => new Model.graphCategoryDetailsData()
                                 {
                                    id = l3.Key.Replace(" \\ ", "").ToLower(),
                                    name = l4.Key,
                                    value = Math.Round(l4.Sum(s => s.Value), 2)
                                 })
                                 .OrderByDescending(l4 => l4.value)
                                 .ToList()

                           })
                        .OrderByDescending(l3 => l3.value)
                        .ToList()

                     })
                     .OrderByDescending(l2 => l2.value)
                     .ToList()

               })
               .OrderByDescending(l1 => l1.value)
               .ToList();

            // LOOP LEVEL 1
            foreach (var level1 in drillDown)
            {
               double colorFactor1 = (double)0.1 / (level1.drillData.Count + 1);

               // LOOP LEVEL 2
               double colorIndex2 = 0;
               foreach (var level2 in level1.drillData)
               {
                  double colorFactor2 = (double)0.1 / (level2.drillData.Count + 1);

                  colorIndex2 += 1;
                  level2.colorIndex = level1.colorIndex;
                  level2.colorBrightness = colorIndex2 * colorFactor1;

                  // LOOP LEVEL 3
                  double colorIndex3 = 0;
                  foreach (var level3 in level2.drillData)
                  {
                     double colorFactor3 = (double)0.1 / (level3.drillData.Count + 1);

                     colorIndex3 += 1;
                     level3.colorIndex = level1.colorIndex;
                     level3.colorBrightness = (colorIndex3 * colorFactor2);

                     // LOOP LEVEL 4
                     double colorIndex4 = 0;
                     foreach (var level4 in level3.drillData)
                     {
                        colorIndex4 += 1;
                        level4.colorIndex = level1.colorIndex;
                        level4.colorBrightness = (colorIndex4 * colorFactor3);
                     }

                  }

               }

            }

            // RESULT
            var result = new Model.graphCategoryDetails() { seriesData = drillDown };
            return result;

         }
         catch (Exception ex) { throw ex; }
      }

      #endregion

   }
}
