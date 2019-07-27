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

      #region GetGraphs_CumulatedEntries
      private bool GetGraphs_CumulatedEntries(Model.graphMainTemp resultData)
      {
         try
         {

            // INITIALIZE
            resultData.cumulatedEntries = new Model.graphCumulatedEntries();

            // GROUP DATA
            var groupsQuery =
               from M in resultData.data.Monthly
               join P in resultData.data.Dimensions.Patterns on M.idPattern equals P.ID
               where M.Type == (short)Categories.Model.enCategoryType.Expense
               group M by new { P.Text } into G
               select new { G.Key.Text, Value = G.Sum(s => s.Value) };
            var groups = groupsQuery.ToList();

            // SERIES
            var totalValue = groups.Select(x => x.Value).Sum();
            var seriesData = groups
               .OrderByDescending(x => x.Value)
               .Select(x => new
               {
                  Name = x.Text,
                  Percentual = Math.Round(x.Value / totalValue * 100, 1),
                  Value = Math.Round(x.Value, 2)
               })
               .ToList();

            // MAIN VALUES
            var percentualLimit = 1.0;
            var mainValues = seriesData.Where(x => x.Percentual >= percentualLimit).ToList();
            resultData.cumulatedEntries.categoryList = mainValues.Select(x => x.Name).ToList();
            resultData.cumulatedEntries.data = mainValues.Select(x => x.Value).ToList();
            resultData.cumulatedEntries.pareto = mainValues.Select(x => x.Percentual).ToList();

            // REMAINING VALUES
            var firstValue = resultData.cumulatedEntries.data.FirstOrDefault();
            seriesData.RemoveAll(x => x.Percentual >= percentualLimit);
            while (seriesData.Count != 0) {
               var remainingValue = seriesData.Select(x => x.Value).Sum();
               if (remainingValue <= (firstValue / 2)) { break; }

               percentualLimit -= 0.1;
               var remainingValues = seriesData.Where(x => x.Percentual >= percentualLimit).ToList();
               resultData.cumulatedEntries.categoryList.AddRange(remainingValues.Select(x => x.Name).ToList());
               resultData.cumulatedEntries.data.AddRange(remainingValues.Select(x => x.Value).ToList());
               resultData.cumulatedEntries.pareto.AddRange(remainingValues.Select(x => x.Percentual).ToList());
               seriesData.RemoveAll(x => x.Percentual >= percentualLimit);
            }

            // CALCULATE PARETO
            var paretoValue = 0.0; var paretoValues = new List<double>();
            foreach (var pareto in resultData.cumulatedEntries.pareto)
            {
               paretoValue += pareto;
               paretoValues.Add(Math.Round(paretoValue, 0));
            }
            resultData.cumulatedEntries.pareto = paretoValues;

            // REST
            var restValue = seriesData.Where(x => x.Percentual < percentualLimit).Select(x => x.Value).Sum();
            if (restValue > 0)
            {
               resultData.cumulatedEntries.categoryList.Add(this.GetTranslation("LABEL_ANALYSIS_REMAINING_SUMMING"));
               resultData.cumulatedEntries.data.Add(Math.Round(restValue, 2));
               resultData.cumulatedEntries.pareto.Add(100);
            }

            // LIMITS
            resultData.cumulatedEntries.limits = new Model.graphLimits();
            if (resultData.cumulatedEntries.data.Count != 0) {
               resultData.cumulatedEntries.limits.minValue = resultData.cumulatedEntries.data.Min();
               resultData.cumulatedEntries.limits.maxValue = resultData.cumulatedEntries.data.Max();
            }
            var difValue = resultData.cumulatedEntries.limits.maxValue - resultData.cumulatedEntries.limits.minValue;
            var stepsValue = Math.Ceiling(difValue / 4); // ROUNDING UP
            resultData.cumulatedEntries.limits.tickInterval = Math.Ceiling(stepsValue / 500) * 500;

            // RESULT
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
