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

      #region GetData
      [Authorize(Roles = "User,Viewer")]
      [Route("data/{year}/{month}")]
      public async Task<IHttpActionResult> GetData(int year, short month)
      {
         try
         {

            // DATA
            var dailyData = await this.GetData_Entries(year, month, enCommandDateGroup.Day);
            var monthlyData = await this.GetData_Entries(year, month, enCommandDateGroup.Month);

            // CATEGORIES
            var categoryList = this.GetData_Categories(dailyData, monthlyData);

            // ACCOUNTS
            var accountList = this.GetData_Accounts(dailyData, monthlyData);

            // PATTERNS
            var patternList = this.GetData_Patterns(dailyData, monthlyData);

            // OTHER DIMENSIONS
            var plannedList = this.GetData_Planned();
            var paidList = this.GetData_Paid();
            // var typeList = this.GetData_Type();

            // RESULT
            var resultData = new Model.viewDataTemp()
            {
               Daily = dailyData,
               Monthly = monthlyData,
               Dimensions = new Model.viewDataDimensions()
               {
                  Accounts = accountList,
                  Categories = categoryList,
                  Patterns = patternList,
                  Planned = plannedList,
                  Paid = paidList
               }
            };
            return Ok(resultData);

         }
         catch (System.Data.Entity.Validation.DbEntityValidationException exEntity) { return this.GetErrorResult(exEntity); }
         catch (Exception ex) { return this.GetErrorResult(ex); }
      }
      #endregion

   }
}
