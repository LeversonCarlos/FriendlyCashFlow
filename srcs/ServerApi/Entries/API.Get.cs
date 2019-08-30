using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Entries
{

   partial class EntriesService
   {

      private IQueryable<EntryData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Entries
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

      internal async Task<ActionResult<List<EntryVM>>> GetDataAsync(short searchYear, short searchMonth, long accountID)
      { return await this.GetDataAsync(searchYear, searchMonth, searchText: "", accountID); }

      internal async Task<ActionResult<List<EntryVM>>> GetDataAsync(string searchText, long accountID)
      { return await this.GetDataAsync(searchYear: 0, searchMonth: 0, searchText, accountID); }

      private async Task<ActionResult<List<EntryVM>>> GetDataAsync(short searchYear, short searchMonth, string searchText, long accountID)
      {
         try
         {
            var user = this.GetService<Helpers.User>();
            var queryPath = "FriendlyCashFlow.ServerApi.Entries.QUERY.EntriesSearch.sql";
            var queryContent = await Helpers.EmbededResource.GetResourceContent(queryPath);
            using (var queryReader = this.GetService<Helpers.DataReaderService>().GetDataReader(queryContent))
            {
               queryReader.AddParameter("@paramResourceID", user.ResourceID);
               queryReader.AddParameter("@paramAccountID", accountID);
               queryReader.AddParameter("@paramSearchYear", searchYear);
               queryReader.AddParameter("@paramSearchMonth", searchMonth);
               queryReader.AddParameter("@paramSearchText", searchText);
               if (!await queryReader.ExecuteReaderAsync()) { return this.WarningResponse("data query error"); }

               var queryResult = await queryReader.GetDataResultAsync<EntryVM>();
               return queryResult;
            }
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      private void ApplySorting(EntryData data)
      {
         try
         {
            data.Sorting = Convert.ToInt64(data.SearchDate.Subtract(new DateTime(1901, 1, 1)).TotalDays);
            data.Sorting += ((decimal)data.EntryID / (decimal)Math.Pow(10, data.EntryID.ToString().Length));
         }
         catch (Exception) { throw; }
      }

   }

   partial class EntriesController
   {

      [HttpGet("search/{searchYear:short}/{searchMonth:short}/{accountID:long}/")]
      [HttpGet("search/{searchYear:short}/{searchMonth:short}/")]
      public async Task<ActionResult<List<EntryVM>>> GetDataAsync(short searchYear, short searchMonth, long accountID = 0)
      {
         var service = this.GetService<EntriesService>();
         return await service.GetDataAsync(searchYear, searchMonth, accountID);
      }

      [HttpGet("search/{searchText}/{accountID:long}/")]
      [HttpGet("search/{searchText}/")]
      public async Task<ActionResult<List<EntryVM>>> GetDataAsync(string searchText, long accountID = 0)
      {
         var service = this.GetService<EntriesService>();
         return await service.GetDataAsync(searchText, accountID);
      }

      /*
      [HttpGet("{id:long}")]
      public async Task<ActionResult<EntryVM>> GetDataAsync(long id)
      {
         var service = this.GetService<EntriesService>();
         return await service.GetDataAsync(id);
      }
      */

   }

}
