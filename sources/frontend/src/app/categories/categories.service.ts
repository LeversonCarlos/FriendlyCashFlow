import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService, StorageService } from '@elesse/shared';
import { CategoryEntity, enCategoryType } from './categories.data';

@Injectable({
   providedIn: 'root'
})
export class CategoriesService {

   constructor(private busy: BusyService,
      private http: HttpClient) {
      this.Cache = new StorageService<enCategoryType, CategoryEntity[]>("CategoriesService");
      this.Cache.InitializeValues(enCategoryType.Income, enCategoryType.Expense);
   }

   private Cache: StorageService<enCategoryType, CategoryEntity[]>;

   public async RefreshCache(): Promise<void> {
      try {
         this.busy.show();
         const values = await this.http.get<CategoryEntity[]>(`api/categories/list`).toPromise();
         if (!values) return;

         const sorter = (a: CategoryEntity, b: CategoryEntity): number => {
            let result = 0;
            if (a.Type > b.Type) result += 100;
            if (a.Type < b.Type) result -= 100;
            if (a.ParentID > b.ParentID) result += 10;
            if (a.ParentID < b.ParentID) result -= 10;
            if (a.Text > b.Text) result += 1;
            if (a.Text < b.Text) result -= 1;
            return result;
         }

         const keys = [enCategoryType.Income, enCategoryType.Expense];
         keys.forEach(key => {
            const value = values
               .sort(sorter)
            this.Cache.SetValue(key, value);
         });

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
