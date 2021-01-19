import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService, MessageService, StorageService } from '@elesse/shared';
import { Observable } from 'rxjs';
import { CategoryEntity, enCategoryType } from './categories.data';

@Injectable({
   providedIn: 'root'
})
export class CategoriesService {

   constructor(private message: MessageService, private busy: BusyService,
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
               .filter(x => x.Type == key)
               .sort(sorter)
            this.Cache.SetValue(key, value);
         });

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public ObserveCategories = (type: enCategoryType): Observable<CategoryEntity[]> =>
      this.Cache.GetObservable(type);

   public async LoadCategory(categoryID: string): Promise<CategoryEntity> {
      try {
         this.busy.show();

         if (!categoryID)
            return null;

         if (categoryID == 'new-income')
            return Object.assign(new CategoryEntity, { Type: enCategoryType.Income });
         if (categoryID == 'new-expense')
            return Object.assign(new CategoryEntity, { Type: enCategoryType.Expense });

         let value = await this.http.get<CategoryEntity>(`api/categories/load/${categoryID}`).toPromise();
         if (!value)
            return null;

         value = Object.assign(new CategoryEntity, value);
         return value;

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async RemoveCategory(category: CategoryEntity) {
      try {

         if (!category)
            return;

         const confirm = await this.message.Confirm("categories.REMOVE_TEXT", "shared.REMOVE_CONFIRM_COMMAND", "shared.REMOVE_CANCEL_COMMAND");
         if (!confirm)
            return

         this.busy.show();

         await this.http.delete(`api/categories/delete/${category.CategoryID}`).toPromise();

         await this.RefreshCache();

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

}
