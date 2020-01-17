using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Accounts
{

   partial class AccountsService
   {

      internal IQueryable<AccountData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Accounts
            .Where(x => x.RowStatus == 1 && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

      public async Task<ActionResult<List<AccountVM>>> GetDataAsync()
      { return await this.GetDataAsync(accountID: 0, searchText: ""); }

      public async Task<ActionResult<List<AccountVM>>> GetDataAsync(string searchText)
      { return await this.GetDataAsync(accountID: 0, searchText: searchText); }

      public async Task<ActionResult<AccountVM>> GetDataAsync(long accountID)
      {
         var dataMessage = await this.GetDataAsync(accountID: accountID, searchText: "");
         var dataValue = this.GetValue(dataMessage);
         if (dataValue == null) { return dataMessage.Result; }
         if (dataValue.Count == 0) { return this.NotFoundResponse(); }
         return this.OkResponse(dataValue[0]);
      }

      private async Task<ActionResult<List<AccountVM>>> GetDataAsync(long accountID, string searchText)
      {
         try
         {

            var query = this.GetDataQuery();
            if (accountID != 0) { query = query.Where(x => x.AccountID == accountID); }
            if (!string.IsNullOrEmpty(searchText))
            { query = query.Where(x => x.AccountID != 0 && x.Text.Contains(searchText)); }

            var data = await query.OrderBy(x => x.Type).ThenBy(x => x.Text).ToListAsync();
            var result = data.Select(x => AccountVM.Convert(x)).ToList();
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class AccountController
   {

      [HttpGet("search")]
      public async Task<ActionResult<List<AccountVM>>> GetDataAsync()
      {
         return await this.GetService<AccountsService>().GetDataAsync();
      }

      [HttpGet("search/{searchText}")]
      public async Task<ActionResult<List<AccountVM>>> GetDataAsync(string searchText)
      {
         return await this.GetService<AccountsService>().GetDataAsync(searchText);
      }

      [HttpGet("{id:long}")]
      public async Task<ActionResult<AccountVM>> GetDataAsync(long id)
      {
         return await this.GetService<AccountsService>().GetDataAsync(id);
      }

   }

}
