using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Analytics
{

   partial class AnalyticsService
   {

      internal async Task<ActionResult<List<ApplicationYieldVM>>> GetApplicationYieldAsync(short searchYear, short searchMonth)
      {
         try
         {
            var data = await this.GetApplicationYieldAsync_Execute(searchYear, searchMonth);
            if (data == null) { return this.WarningResponse("data query error"); }
            return data;
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private async Task<List<ApplicationYieldVM>> GetApplicationYieldAsync_Execute(short searchYear, short searchMonth)
      {
         try
         {
            var queryPath = "FriendlyCashFlow.ServerApi.Analytics.QUERY.ApplicationYield.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", this.GetService<Helpers.User>().ResourceID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);

               if (!await queryReader.ExecuteReaderAsync()) { return null; }
               return await queryReader.GetDataResultAsync<ApplicationYieldVM>();
            }
         }
         catch (Exception) { throw; }
      }

   }

   partial class AnalyticsController
   {

      [HttpGet("applicationYield/{searchYear}/{searchMonth}")]
      public async Task<ActionResult<List<ApplicationYieldVM>>> GetApplicationYield(short searchYear, short searchMonth)
      {
         return await this.GetService<AnalyticsService>().GetApplicationYieldAsync(searchYear, searchMonth);
      }

   }

   public class ApplicationYieldVM
   {
      public DateTime Date { get; set; }
      public string SmallText { get { return this.Date.ToString("MMM").ToUpper(); } }
      public long AccountID { get; set; }
      public string AccountText { get; set; }
      public decimal Gain { get; set; }
      public decimal Percentual { get; set; }
   }

}
