using FriendlyCashFlow.API.Entries;
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

      internal async Task<ActionResult<List<EntryVM>>> GetEntriesAsync()
      {
         try
         {
            var user = this.GetService<Helpers.User>();
            var queryPath = "FriendlyCashFlow.ServerApi.Dashboard.QUERY.Entries.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", user.ResourceID);
               if (!await queryReader.ExecuteReaderAsync()) { return this.WarningResponse("data query error"); }

               var queryResult = await queryReader.GetDataResultAsync<EntryVM>();
               return queryResult;
            }
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class DashboardController
   {

      [HttpGet("entries")]
      public async Task<ActionResult<List<EntryVM>>> GetEntriesAsync()
      {
         return await this.GetService<DashboardService>().GetEntriesAsync();
      }

   }

}
