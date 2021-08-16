using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Budget
{

   partial class BudgetService
   {

      internal IQueryable<BudgetData> GetDataQuery()
      {
         var user = this.GetService<Helpers.User>();
         return this.dbContext.Budget
            .Where(x => x.RowStatus == (short)Base.enRowStatus.Active && x.ResourceID == user.ResourceID)
            .AsQueryable();
      }

      internal async Task<ActionResult<List<BudgetFlowVM>>> GetDataAsync()
      {
         var budgetList = await this
            .GetDataQuery()
            .Include(x => x.PatternDetails)
            .Include(x => x.PatternDetails.CategoryDetails)
            .ToListAsync();

         var flowList = budgetList
            .GroupBy(x => x.PatternDetails.CategoryID)
            .Select(x => new
            {
               CategoryDetails = x.Min(g => g.PatternDetails.CategoryDetails),
               BudgetList = budgetList
                  .Where(e => e.PatternDetails.CategoryID == x.Key)
                  .ToList()
            })
            .OrderBy(x => x.CategoryDetails.HierarchyText)
            .Select(x => new BudgetFlowVM
            {
               CategoryRow = Categories.CategoryVM.Convert(x.CategoryDetails),
               BudgetList = x.BudgetList
                  .Select(g => Budget.BudgetVM.Convert(g, g.PatternDetails))
                  .ToArray(),
               Value = x.BudgetList
                  .Select(g => g.Value)
                  .Sum()
            })
            .ToList();

         return flowList;
      }

      internal async Task<ActionResult<BudgetVM>> GetDataAsync(long budgetID)
      {
         try
         {

            var data = await this
               .GetDataQuery()
               .Where(x => x.BudgetID == budgetID)
               .FirstOrDefaultAsync();

            var patternData = await this.GetService<Patterns.PatternsService>()
               .GetDataQuery()
               .Include(x => x.CategoryDetails)
               .Where(x => x.PatternID == data.PatternID)
               .FirstOrDefaultAsync();

            var viewModel = BudgetVM.Convert(data, patternData);

            return this.OkResponse(viewModel);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class BudgetController
   {

      [HttpGet("flow")]
      public async Task<ActionResult<List<BudgetFlowVM>>> GetDataAsync()
      {
         return await this.GetService<BudgetService>().GetDataAsync();
      }

      [HttpGet("budget/{id:long}")]
      public async Task<ActionResult<BudgetVM>> GetDataAsync(long id)
      {
         return await this.GetService<BudgetService>().GetDataAsync(id);
      }

   }

}
