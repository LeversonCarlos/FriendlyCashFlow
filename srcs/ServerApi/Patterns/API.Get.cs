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


      internal async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(Categories.enCategoryType categoryType)
      { return await this.GetPatternsAsync(patternID: 0, categoryType, searchText: ""); }

      internal async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(Categories.enCategoryType categoryType, string searchText)
      { return await this.GetPatternsAsync(patternID: 0, categoryType, searchText: searchText); }

      internal async Task<ActionResult<PatternVM>> GetPatternAsync(long patternID)
      {
         var dataMessage = await this.GetPatternsAsync(patternID: patternID, categoryType: Categories.enCategoryType.None, searchText: "");
         var dataValue = this.GetValue(dataMessage);
         if (dataValue == null) { return dataMessage.Result; }
         if (dataValue.Count == 0) { return this.NotFoundResponse(); }
         return this.OkResponse(dataValue[0]);
      }

      private async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(long patternID, Categories.enCategoryType categoryType, string searchText)
      {
         try
         {

            var query = this.GetDataQuery();
            if (patternID != 0) { query = query.Where(x => x.PatternID == patternID); }
            if (categoryType != Categories.enCategoryType.None) { query = query.Where(x => x.Type == (short)categoryType); }
            if (!string.IsNullOrEmpty(searchText))
            { query = query.Where(x => x.PatternID != 0 && x.Text.Contains(searchText)); }

            var data = await query
               .Include(x => x.CategoryDetails)
               .OrderByDescending(x => x.Count)
               .ThenBy(x => x.Text)
               .Take(500)
               .ToListAsync();
            var result = data.Select(x => PatternVM.Convert(x)).ToList();
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

      internal async Task<long> GetPatternIDAsync(Entries.EntryVM value)
      {
         return await this.GetDataQuery()
            .Where(x => x.Type == (short)value.Type && x.CategoryID == (value.CategoryID ?? 0) && x.Text == value.Text)
            .Select(x => x.PatternID)
            .FirstOrDefaultAsync();
      }

   }

   partial class PatternsController
   {

      [HttpGet("search/{categoryType}")]
      public async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(Categories.enCategoryType categoryType)
      {
         return await this.GetService<PatternsService>().GetPatternsAsync(categoryType);
      }

      [HttpGet("search/{categoryType}/{searchText}")]
      public async Task<ActionResult<List<PatternVM>>> GetPatternsAsync(Categories.enCategoryType categoryType, string searchText)
      {
         return await this.GetService<PatternsService>().GetPatternsAsync(categoryType, searchText);
      }

      [HttpGet("{id:long}")]
      public async Task<ActionResult<PatternVM>> GetPatternAsync(long id)
      {
         return await this.GetService<PatternsService>().GetPatternAsync(id);
      }

   }

}
