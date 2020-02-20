using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Dashboard
{

   partial class DashboardService
   {

      internal async Task<ActionResult<List<BalanceVM>>> GetBalanceAsync(short searchYear, short searchMonth, bool excludeTransfers)
      {
         try
         {
            var user = this.GetService<Helpers.User>();
            var queryPath = "FriendlyCashFlow.ServerApi.Dashboard.QUERY.Balance.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", user.ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);
               queryReader.AddParameter("@paramExcludeTransfers", (excludeTransfers ? 1 : 0));
               if (!await queryReader.ExecuteReaderAsync()) { return this.WarningResponse("data query error"); }

               var queryResult = await queryReader.GetDataResultAsync<BalanceVM>();
               return queryResult;
            }
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class DashboardController
   {

      [HttpGet("balance/{searchYear}/{searchMonth}/{excludeTransfers}")]
      public async Task<ActionResult<List<BalanceVM>>> GetBalanceAsync(short searchYear, short searchMonth, short excludeTransfers)
      {
         return await this.GetService<DashboardService>().GetBalanceAsync(searchYear, searchMonth, excludeTransfers==1);
      }

   }

}
