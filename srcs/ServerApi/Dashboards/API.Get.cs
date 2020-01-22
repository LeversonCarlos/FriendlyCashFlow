using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Dashboards
{

   partial class DashboardsService
   {

      internal async Task<ActionResult<List<BalanceVM>>> GetBalanceAsync(short searchYear, short searchMonth)
      {
         try
         {
            var user = this.GetService<Helpers.User>();
            var queryPath = "FriendlyCashFlow.ServerApi.Dashboards.QUERY.Balance.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", user.ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);
               if (!await queryReader.ExecuteReaderAsync()) { return this.WarningResponse("data query error"); }

               var queryResult = await queryReader.GetDataResultAsync<BalanceVM>();
               return queryResult;
            }
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class DashboardsController
   {

      [HttpGet("balance/{searchYear}/{searchMonth}/")]
      public async Task<ActionResult<List<BalanceVM>>> GetBalanceAsync(short searchYear, short searchMonth)
      {
         return await this.GetService<DashboardsService>().GetBalanceAsync(searchYear, searchMonth);
      }

   }

}
