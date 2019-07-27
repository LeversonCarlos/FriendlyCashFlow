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

      #region GetGraphs_DailyFlow
      private bool GetGraphs_DailyFlow(Model.graphMainTemp resultData)
      {
         try
         {

            // INITIALIZE
            resultData.dailyFlow = new Model.graphDailyFlow();
            var initialDay = resultData.data.currentMonth.InitialDate.Day;
            var finalDay = resultData.data.currentMonth.FinalDate.Day;
            double unplannedValue = 0; double paidValue = 0; double unpaidValue = 0;

            // INITIALIZE VALUES
            resultData.dailyFlow.category.Add(0);
            resultData.dailyFlow.unplanned.data.Add(unplannedValue);
            resultData.dailyFlow.unpaid.data.Add(unpaidValue);
            resultData.dailyFlow.paid.data.Add(paidValue);

            // DAILY LOOP 
            var initialModelIndex = 0;
            for (var currentDay = initialDay; currentDay <= finalDay; currentDay++)
            {
               resultData.dailyFlow.category.Add(currentDay);

               // LOOP DAILY MODEL STARTING FROM WHERE STOPED LAST TIME
               for (var currentModelIndex = initialModelIndex; currentModelIndex < resultData.data.Daily.Count; currentModelIndex++)
               {
                  initialModelIndex = currentModelIndex;
                  var currentModel = resultData.data.Daily[currentModelIndex];
                  if (currentModel.SearchDay > currentDay) { break; }
                  if (currentModel.Type != (short)Categories.Model.enCategoryType.Expense) { continue; }

                  if (currentModel.Planned == 0) { unplannedValue += currentModel.Value; }
                  else {
                     if (currentModel.Paid == 0) { unpaidValue += currentModel.Value; }
                     else { paidValue += currentModel.Value; }
                  }
               }

               // APPLY VALUES
               resultData.dailyFlow.unplanned.data.Add(unplannedValue);
               resultData.dailyFlow.unpaid.data.Add(unpaidValue);
               resultData.dailyFlow.paid.data.Add(paidValue);

            }

            // SERIES DETAILS
            resultData.dailyFlow.unplanned.name = this.GetTranslation("LABEL_ANALYSIS_DIMENSION_UNPLANNED");
            resultData.dailyFlow.unpaid.name = this.GetTranslation("LABEL_ANALYSIS_DIMENSION_UNPAID");
            resultData.dailyFlow.paid.name = this.GetTranslation("LABEL_ANALYSIS_DIMENSION_PAID");

            // TODAY
            if (resultData.data.currentMonth.Year == DateTime.Now.Year && resultData.data.currentMonth.Month == DateTime.Now.Month)
            {
               resultData.dailyFlow.currentDay = DateTime.Now.Day;
            }

            // RESULT
            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
