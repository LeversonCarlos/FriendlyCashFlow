using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Elesse.Categories
{

   partial class CategoryService
   {

      public async Task<IActionResult> UpdateAsync(UpdateVM updateVM)
      {

         // VALIDATE PARAMETERS
         if (updateVM == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_UPDATE_PARAMETER });

         // VALIDATE PARENT
         // TODO

         // VALIDATE DUPLICITY
         var categoriesList = await _CategoryRepository.SearchCategoriesAsync(updateVM.Type, updateVM.ParentID, updateVM.Text);
         if (categoriesList.Any(x => x.CategoryID != updateVM.CategoryID))
            return new BadRequestObjectResult(new string[] { WARNINGS.CATEGORY_TEXT_ALREADY_USED });

         // LOCATE CATEGORY
         var category = (CategoryEntity)(await _CategoryRepository.LoadCategoryAsync(updateVM.CategoryID));
         if (category == null)
            return new BadRequestObjectResult(new string[] { WARNINGS.CATEGORY_NOT_FOUND });

         // APPLY CHANGES
         category.Text = updateVM.Text;
         category.Type = updateVM.Type;
         category.ParentID = updateVM.ParentID;

         // SAVE CHANGES
         await _CategoryRepository.UpdateCategoryAsync(category);

         // RESULT
         return new OkResult();
      }

   }

   partial interface ICategoryService
   {
      Task<IActionResult> UpdateAsync(UpdateVM updateVM);
   }

   partial struct WARNINGS
   {
      internal const string INVALID_UPDATE_PARAMETER = "INVALID_UPDATE_PARAMETER";
      internal const string CATEGORY_NOT_FOUND = "CATEGORY_NOT_FOUND";
   }

}
