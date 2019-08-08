
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Categories
{
   partial class CategoriesService
   {

      private IQueryable<CategoryData> GetDataQuery()
      {
         return this.dbContext.Categories
            .Where(x => x.RowStatus == 1 && x.ResourceID == resourceID)
            .AsQueryable();
      }

      public async Task<ActionResult<List<CategoryVM>>> GetDataAsync(enCategoryType categoryType)
      { return await this.GetDataAsync(categoryID: 0, categoryType: categoryType, searchText: ""); }

      public async Task<ActionResult<List<CategoryVM>>> GetDataAsync(enCategoryType categoryType, string searchText)
      { return await this.GetDataAsync(categoryID: 0, categoryType: categoryType, searchText: searchText); }

      public async Task<ActionResult<CategoryVM>> GetDataAsync(long categoryID)
      {
         var dataMessage = await this.GetDataAsync(categoryID: categoryID, categoryType: enCategoryType.None, searchText: "");
         var dataValue = this.GetValue(dataMessage);
         if (dataValue == null) { return dataMessage.Result; }
         if (dataValue.Count == 0) { return this.NotFoundResponse(); }
         return this.OkResponse(dataValue[0]);
      }

      private async Task<ActionResult<List<CategoryVM>>> GetDataAsync(long categoryID, enCategoryType categoryType, string searchText)
      {
         try
         {

            var query = this.GetDataQuery();
            if (categoryID != 0) { query = query.Where(x => x.CategoryID == categoryID); }
            if (categoryType != enCategoryType.None) { query = query.Where(x => x.Type == (short)categoryType); }
            if (!string.IsNullOrEmpty(searchText))
            { query = query.Where(x => x.HierarchyText.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)); }

            var data = await query.ToListAsync();
            var result = data
               .OrderBy(x => x.HierarchyText)
               .Select(x => CategoryVM.Convert(x))
               .ToList();
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
