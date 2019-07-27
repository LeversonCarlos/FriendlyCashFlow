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

      #region GetGraphs
      [HttpPost]
      [Authorize(Roles = "User,Viewer")]
      [Route("graph")]
      public async Task<IHttpActionResult> GetGraphs(Model.viewData param)
      {
         try
         {

            // INITIALIZE
            var resultData = new Model.graphMainTemp();

            // DATA
            if (!await this.GetGraphs_ApplyFilter(resultData, param)) { return BadRequest(); }
            if (!this.GetGraphs_MainCategories(resultData)) { return BadRequest(); }

            // GRAPHS
            if (!this.GetGraphs_DailyFlow(resultData)) { return BadRequest(); }
            if (!this.GetGraphs_CategoryDetails(resultData)) { return BadRequest(); }
            if (!this.GetGraphs_CategoryTrend(resultData)) { return BadRequest(); }
            if (!this.GetGraphs_CategoryGoal(resultData)) { return BadRequest(); }
            if (!this.GetGraphs_BalanceTrend(resultData)) { return BadRequest(); }
            if (!this.GetGraphs_CumulatedEntries(resultData)) { return BadRequest(); }

            // RESULT
            return Ok(resultData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion 

      #region GetGraphs_ApplyFilter
      private async Task<bool> GetGraphs_ApplyFilter(Model.graphMainTemp resultData, Model.viewData param)
      {
         try
         {

            // GET DATA
            var dataResult = ((System.Web.Http.Results.OkNegotiatedContentResult<Model.viewDataTemp>)await this.GetData(param.currentMonth.Year, param.currentMonth.Month));
            resultData.data = dataResult.Content;
            resultData.data.currentMonth = param.currentMonth;

            // FILTER DAILY
            resultData.data.Daily.ForEach(x => {
               var selAccount = param.Dimensions.Accounts[(int)x.idAccount].Selected;
               var selCategory = x.idCategory < 0 || param.Dimensions.Categories[(int)x.idCategory].Selected;
               var selPattern = param.Dimensions.Patterns[(int)x.idPattern].Selected;
               var selPlanned = param.Dimensions.Planned[x.Planned].Selected;
               var selPaid = param.Dimensions.Paid[x.Paid].Selected;
               x.Selected = (selAccount && selCategory && selPattern && selPlanned && selPaid);
               if (!x.Selected) { var t = 1; }
            });
            resultData.data.Daily = resultData.data.Daily
               .Where(x => x.Selected)
               .OrderBy(x => x.SearchDate)
               .ThenBy(x => x.idCategory).ThenBy(x => x.idAccount).ThenBy(x => x.idPattern)
               .ThenBy(x => x.Planned).ThenBy(x => x.Paid).ThenBy(x => x.Type)
               .ToList();

            // FILTER MONTHLY
            resultData.data.Monthly.ForEach(x => {
               var selAccount = param.Dimensions.Accounts[(int)x.idAccount].Selected;
               var selCategory = x.idCategory < 0 || param.Dimensions.Categories[(int)x.idCategory].Selected;
               var selPattern = param.Dimensions.Patterns[(int)x.idPattern].Selected;
               var selPlanned = param.Dimensions.Planned[x.Planned].Selected;
               var selPaid = param.Dimensions.Paid[x.Paid].Selected;
               x.Selected = (selAccount && selCategory && selPattern && selPlanned && selPaid);
            });
            resultData.data.Monthly = resultData.data.Monthly
               .Where(x => x.Selected)
               .OrderBy(x => x.SearchDate)
               .ThenBy(x => x.idCategory).ThenBy(x => x.idAccount).ThenBy(x => x.idPattern)
               .ThenBy(x => x.Planned).ThenBy(x => x.Paid).ThenBy(x => x.Type)
               .ToList();

            return true;
         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

      #region GetGraphs_MainCategories
      private bool GetGraphs_MainCategories(Model.graphMainTemp resultData)
      {
         try
         {
            resultData.mainCategories = new Dictionary<string, short>();

            var mainCategoriesQuery =
               from M in resultData.data.Monthly
               join C in resultData.data.Dimensions.Categories on M.idCategory equals C.ID
               where M.Type == (short)Categories.Model.enCategoryType.Expense
               group M by C.ParentText into G
               select new { ParentText = G.Key, Value = G.Sum(s => s.Value) };
            var mainCategories = mainCategoriesQuery.OrderByDescending(x => x.Value).ToList();

            short colorIndex = 0;
            foreach (var category in mainCategories)
            {
               resultData.mainCategories.Add(category.ParentText, colorIndex);
               colorIndex++;
               if (colorIndex >= 10) { colorIndex = 0; }
            }

            return true;

         }
         catch (Exception ex) { throw ex; }
      }
      #endregion

   }
}
