import { Injectable } from '@angular/core';
import { enCategoryType } from '@elesse/categories';
import { StorageService } from '@elesse/shared';
import { PatternEntity } from '../model/patterns.model';

@Injectable({
   providedIn: 'root'
})
export class PatternsCacheService extends StorageService<enCategoryType, PatternEntity[]> {

   constructor() {
      super("PatternsCache");
      this.InitializeValues(...this.CategoryTypes);
   }

   private get CategoryTypes(): enCategoryType[] {
      return [
         enCategoryType.Income,
         enCategoryType.Expense
      ];
   }

   public SetPatterns(patterns: PatternEntity[]) {

      // SORTER FUNCTION
      const sorter = (a: PatternEntity, b: PatternEntity): number => {
         let result = 0;
         if (a.Text > b.Text) result += 1;
         if (a.Text < b.Text) result -= 1;
         return result;
      }

      // LOOP THROUGH CATEGORY TYPES
      for (const categoryType of this.CategoryTypes) {

         // FILTER, SORT AND PARSE OBJECTS
         const value = patterns
            .filter(pattern => pattern.Type == categoryType)
            .map(pattern => PatternEntity.Parse(pattern))
            .sort(sorter);

         // SET VALUE
         this.SetValue(categoryType, value);

      }

   }

}
