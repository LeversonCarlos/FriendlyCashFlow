using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

      internal async Task<ActionResult<List<EntryFlowVM>>> GetDataAsync(short searchYear, short searchMonth, long accountID)
      {
         var entryListMessage = await this.GetDataAsync(searchYear, searchMonth, searchText: "", accountID);
         var entryList = this.GetValue(entryListMessage);
         if (entryList == null) { return entryListMessage.Result; }
         var flowList = entryList
            .GroupBy(x => x.DueDate.Day)
            .Select(x => new
            {
               Day = x.Key,
               EntryList = entryList.Where(e => e.DueDate.Day == x.Key).ToList()
            })
            .Select(x => new EntryFlowVM
            {
               Day = x.Day.ToString().PadLeft(2, "0".ToCharArray()[0]),
               EntryList = x.EntryList.OrderBy(e => e.Sorting).ToArray(),
               BalanceTotalValue = x.EntryList.OrderByDescending(e => e.Sorting).Select(e => e.BalanceTotalValue).FirstOrDefault(),
               BalancePaidValue = x.EntryList.OrderByDescending(e => e.Sorting).Select(e => e.BalancePaidValue).FirstOrDefault()
            })
            .ToList();
         return flowList;
      }

      internal async Task<ActionResult<List<EntryVM>>> GetDataAsync(string searchText, long accountID)
      { return await this.GetDataAsync(searchYear: 0, searchMonth: 0, searchText, accountID); }

      internal async Task<ActionResult<EntryVM>> GetDataAsync(long entryID)
      {
         try
         {

            var data = await this.GetDataQuery()
               .Where(x => x.EntryID == entryID)
               .FirstOrDefaultAsync();
            var viewModel = EntryVM.Convert(data);

            if (data.AccountID.HasValue)
            {
               var accountMessage = await this.GetService<Accounts.AccountsService>().GetDataAsync(data.AccountID.Value);
               viewModel.AccountRow = this.GetValue(accountMessage);
            }

            var categoryMessage = await this.GetService<Categories.CategoriesService>().GetDataAsync(data.CategoryID);
            viewModel.CategoryRow = this.GetValue(categoryMessage);

            return this.OkResponse(viewModel);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

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

      [HttpGet("flow/{searchYear}/{searchMonth}/{accountID}/")]
      [HttpGet("flow/{searchYear}/{searchMonth}/")]
      public async Task<ActionResult<List<EntryFlowVM>>> GetDataAsync(short searchYear, short searchMonth, long accountID = 0)
      {
         return await this.GetService<EntriesService>().GetDataAsync(searchYear, searchMonth, accountID);
      }

      [HttpGet("search/{searchText}/{accountID}/")]
      [HttpGet("search/{searchText}/")]
      public async Task<ActionResult<List<EntryVM>>> GetDataAsync(string searchText, long accountID = 0)
      {
         return await this.GetService<EntriesService>().GetDataAsync(searchText, accountID);
      }

      [HttpGet("entry/{id:long}")]
      public async Task<ActionResult<EntryVM>> GetDataAsync(long id)
      {
         return await this.GetService<EntriesService>().GetDataAsync(id);
      }

   }

}
