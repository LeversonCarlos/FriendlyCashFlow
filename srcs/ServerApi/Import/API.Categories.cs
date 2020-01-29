using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyCashFlow.API.Import
{

   partial class ImportService
   {

      internal async Task<ActionResult<bool>> CreateCategoriesAsync(ImportVM value)
      {
         try
         {
            var categoriesService = this.GetService<Categories.CategoriesService>();
            if (value.Entries == null) { return this.OkResponse(true); }

            // LOAD DATA
            value.Categories = new List<Categories.CategoryVM>();
            var categoryTypes = new Categories.enCategoryType[] { Categories.enCategoryType.Income, Categories.enCategoryType.Expense };
            foreach (var categoryType in categoryTypes)
            {
               var loadMessage = await categoriesService.GetDataAsync(categoryType);
               var loadResult = this.GetValue(loadMessage);
               if (loadResult == null) { return loadMessage.Result; }
               value.Categories.AddRange(loadResult);
            }

            // GROUP ALL CATEGORY TEXTS
            var incomeCategoryTexts = value.Entries
               .Where(x => x.Type == Categories.enCategoryType.Income).Select(x => x.Category)
               .GroupBy(x => x).Select(x => x.Key).OrderBy(x => x)
               .ToList();
            incomeCategoryTexts.RemoveAll(x => value.Categories.Select(a => a.HierarchyText).Contains(x));
            var expenseCategoryTexts = value.Entries
               .Where(x => x.Type == Categories.enCategoryType.Expense).Select(x => x.Category)
               .GroupBy(x => x).Select(x => x.Key).OrderBy(x => x)
               .ToList();
            expenseCategoryTexts.RemoveAll(x => value.Categories.Select(a => a.HierarchyText).Contains(x));

            // GET CATEGORY FUNCTION
            var getCategoryID = new Func<Categories.enCategoryType, long?, string, long?>((categoryType, parentID, categoryText) => value.Categories
                  .Where(a => a.Type == categoryType && a.ParentID == parentID && a.Text == categoryText)
                  .Select(a => a.CategoryID)
                  .FirstOrDefault());
            var getHierarchyCategoryID = new Func<Categories.enCategoryType, string, long?>((categoryType, categoryText) => value.Categories
                  .Where(a => a.Type == categoryType && a.HierarchyText == categoryText)
                  .Select(a => a.CategoryID)
                  .FirstOrDefault());

            // NEW CATEGORY FUNCTION
            var newCategory = new Func<Categories.enCategoryType, long?, string, Task>(async (categoryType, parentID, hierarchyText) =>
            {
               var categoryTexts = hierarchyText.Split(" / ", StringSplitOptions.RemoveEmptyEntries);
               foreach (var categoryText in categoryTexts)
               {

                  // TRY TO LOCATE
                  var categoryID = getCategoryID(categoryType, parentID, categoryText);
                  if (!categoryID.HasValue || categoryID.Value == 0)
                  {

                     // ADD A NEW ONE
                     var createParam = new Categories.CategoryVM
                     {
                        ParentID = parentID,
                        Text = categoryText,
                        Type = categoryType
                     };
                     var createMessage = await categoriesService.CreateAsync(createParam);
                     var createResult = this.GetValue(createMessage);
                     if (createResult != null)
                     {
                        value.Categories.Add(createResult);
                        categoryID = createResult.CategoryID;
                     }

                  }

                  parentID = categoryID;
               }
            });

            // CHECK FOR NEW CATEGORIES
            foreach (var categoryText in incomeCategoryTexts)
            { await newCategory(Categories.enCategoryType.Income, 0, categoryText); }
            foreach (var categoryText in expenseCategoryTexts)
            { await newCategory(Categories.enCategoryType.Expense, 0, categoryText); }

            // MARK CATEGORIES ON ENTRIES
            if (value.Entries != null)
            { value.Entries.ForEach(x => x.CategoryID = getHierarchyCategoryID(x.Type, x.Category)); }
            if (value.Entries.Any(x => !x.CategoryID.HasValue || x.CategoryID.Value == 0))
            { return this.WarningResponse("IMPORT_SOME_CATEGORIES_COULD_NOT_BE_DEFINED"); }

            // RESULT
            return this.OkResponse(true);
         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

}
