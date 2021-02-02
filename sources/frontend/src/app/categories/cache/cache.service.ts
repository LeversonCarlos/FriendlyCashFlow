import { Injectable } from '@angular/core';
import { StorageService } from '@elesse/shared';
import { CategoryEntity, enCategoryType } from '../model/categories.model';

@Injectable({
   providedIn: 'root'
})
export class CategoriesCache extends StorageService<enCategoryType, CategoryEntity[]> {

   constructor() {
      super("CategoriesCache");
      this.InitializeValues(...this.CategoryTypes);
   }

   private get CategoryTypes(): enCategoryType[] {
      return [
         enCategoryType.Income,
         enCategoryType.Expense
      ];
   }

   public SetCategories(categories: CategoryEntity[]) {

      // SORTER FUNCTION
      const sorter = (a: CategoryEntity, b: CategoryEntity): number => {
         let result = 0;
         if (a.HierarchyText > b.HierarchyText) result += 1;
         if (a.HierarchyText < b.HierarchyText) result -= 1;
         return result;
      }

      // LOOP THROUGH CATEGORY TYPES
      for (const categoryType of this.CategoryTypes) {

         // FILTER, SORT AND PARSE OBJECTS
         const value = categories
            .filter(category => category.Type == categoryType)
            .sort(sorter);

         // SET VALUE
         this.SetValue(categoryType, value);

      }

   }

}
