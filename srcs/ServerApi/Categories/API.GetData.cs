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
            {
               searchText = System.Web.HttpUtility.UrlDecode(searchText);
               query = query.Where(x => x.HierarchyText.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));
            }

            var categoryList = await query.ToListAsync();
            var parentIDs = categoryList.Where(x => x.ParentID.HasValue).Select(x => x.ParentID.Value).Distinct().ToArray();
            var parentList = await this.GetDataQuery().Where(x => parentIDs.Contains(x.CategoryID)).ToListAsync();

            var result = categoryList
               .OrderBy(x => x.HierarchyText)
               .Select(x => new { Category = x, ParentRow = parentList.Where(p => p.CategoryID == x.ParentID).FirstOrDefault() })
               .Select(x => CategoryVM.Convert(x.Category, x.ParentRow))
               .ToList();
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class CategoriesController
   {

      [HttpGet("search/{categoryType}")]
      public async Task<ActionResult<List<CategoryVM>>> GetDataAsync(enCategoryType categoryType)
      {
         var service = this.GetService<CategoriesService>();
         return await service.GetDataAsync(categoryType);
      }

      [HttpGet("search/{categoryType}/{searchText}")]
      public async Task<ActionResult<List<CategoryVM>>> GetDataAsync(enCategoryType categoryType, string searchText)
      {
         var service = this.GetService<CategoriesService>();
         return await service.GetDataAsync(categoryType, searchText);
      }

      [HttpGet("{id:long}")]
      public async Task<ActionResult<CategoryVM>> GetDataAsync(long id)
      {
         var service = this.GetService<CategoriesService>();
         return await service.GetDataAsync(id);
      }

   }

}
