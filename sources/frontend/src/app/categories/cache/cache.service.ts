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

}
