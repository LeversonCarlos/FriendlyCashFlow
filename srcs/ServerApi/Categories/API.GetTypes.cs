using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Categories
{

   partial class CategoriesService
   {

      public async Task<ActionResult<List<CategoryTypeVM>>> GetCategoryTypesAsync()
      {
         try
         {
            var categoryText = $"{"Categories".ToUpper()}_{"enCategoryType".ToUpper()}";
            var categoryTypes = new enCategoryType[] { enCategoryType.Income, enCategoryType.Expense };
            var result = categoryTypes
                .Select(x => new CategoryTypeVM
                {
                   Value = x,
                   Text = this.GetTranslation($"{categoryText}_{x.ToString().ToUpper()}")
                })
                .OrderBy(x => x.Text)
                .ToList();
            result = await Task.FromResult(result); // just to keep it async
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class CategoriesController
   {
      [HttpGet("types")]
      public async Task<ActionResult<List<CategoryTypeVM>>> GetCategoryTypeAsync()
      {
         var service = this.GetService<CategoriesService>();
         return await service.GetCategoryTypesAsync();
      }
   }

}
