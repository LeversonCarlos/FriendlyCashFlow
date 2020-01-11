using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Patterns
{

   partial class PatternsService
   {

      private IQueryable<PatternData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Patterns
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

      public async Task<ActionResult<List<PatternVM>>> GetPatternsAsync()
      { return await this.GetPatternsAsync(patternID: 0, searchText: ""); }

      public async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(string searchText)
      { return await this.GetPatternsAsync(patternID: 0, searchText: searchText); }

      public async Task<ActionResult<PatternVM>> GetPatternAsync(long patternID)
      {
         var dataMessage = await this.GetPatternsAsync(patternID: patternID, searchText: "");
         var dataValue = this.GetValue(dataMessage);
         if (dataValue == null) { return dataMessage.Result; }
         if (dataValue.Count == 0) { return this.NotFoundResponse(); }
         return this.OkResponse(dataValue[0]);
      }

      private async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(long patternID, string searchText)
      {
         try
         {

            var query = this.GetDataQuery();
            if (patternID != 0) { query = query.Where(x => x.PatternID == patternID); }
            if (!string.IsNullOrEmpty(searchText))
            { query = query.Where(x => x.PatternID != 0 && x.Text.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)); }

            var data = await query
               .Include(x=> x.CategoryDetails)
               .OrderByDescending(x => x.Count)
               .ThenBy(x => x.Text)
               .ToListAsync();
            var result = data.Select(x => PatternVM.Convert(x)).ToList();
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      internal async Task<long> GetPatternIDAsync(Entries.EntryVM value)
      {
         return await this.GetDataQuery()
            .Where(x => x.Type == (short)value.Type && x.CategoryID == value.CategoryID && x.Text == value.Text)
            .Select(x => x.PatternID)
            .FirstOrDefaultAsync();
      }

   }

   partial class PatternsController
   {

      [HttpGet("search")]
      public async Task<ActionResult<List<PatternVM>>> GetPatternsAsync()
      {
         return await this.GetService<PatternsService>().GetPatternsAsync();
      }

      [HttpGet("search/{searchText}")]
      public async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(string searchText)
      {
         return await this.GetService<PatternsService>().GetPatternsAsync(searchText);
      }

      [HttpGet("{id:long}")]
      public async Task<ActionResult<PatternVM>> GetPatternAsync(long id)
      {
         return await this.GetService<PatternsService>().GetPatternAsync(id);
      }

   }

}
