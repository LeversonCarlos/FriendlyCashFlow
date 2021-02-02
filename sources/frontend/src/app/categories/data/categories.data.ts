import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BusyService, LocalizationService, MessageService, StorageService } from '@elesse/shared';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CategoriesCache } from '../cache/cache.service';
import { CategoryEntity, CategoryType, enCategoryType } from '../model/categories.model';

@Injectable({
   providedIn: 'root'
})
export class CategoriesData {

   constructor(private localization: LocalizationService, private message: MessageService, private busy: BusyService,
      private http: HttpClient) {
      this.Cache = new CategoriesCache();
   }

   private Cache: CategoriesCache;
   public SelectedType: enCategoryType = enCategoryType.Income;

   public async RefreshCategories(): Promise<void> {
      try {
         this.busy.show();

         const url = `api/categories/list`;
         const values = await this.http.get<CategoryEntity[]>(url).toPromise();
         if (!values)
            return;

         this.Cache.SetCategories(values);

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public ObserveCategories = (type: enCategoryType): Observable<CategoryEntity[]> =>
      this.Cache.GetObservable(type)
         .pipe(
            map(entries => entries?.map(entry => CategoryEntity.Parse(entry)) ?? null)
         );

   public GetCategories = (type: enCategoryType): CategoryEntity[] =>
      this.Cache.GetValue(type);

   public async LoadCategory(categoryID: string): Promise<CategoryEntity> {
      try {
         this.busy.show();

         if (!categoryID)
            return null;

         if (categoryID == 'new')
            return Object.assign(new CategoryEntity, { Type: this.SelectedType });

         let value = await this.http.get<CategoryEntity>(`api/categories/load/${categoryID}`).toPromise();
         if (!value)
            return null;

         value = Object.assign(new CategoryEntity, value);
         return value;

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async SaveCategory(category: CategoryEntity): Promise<boolean> {
      try {
         this.busy.show();

         if (!category)
            return false;

         if (category.CategoryID == null)
            await this.http.post("api/categories/insert", category).toPromise();
         else
            await this.http.put("api/categories/update", category).toPromise();

         await this.RefreshCategories();

         return true;

      }
      catch { return false; /* error absorber */ }
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

         await this.RefreshCategories();

      }
      catch { /* error absorber */ }
      finally { this.busy.hide(); }
   }

   public async GetCategoryTypes(): Promise<CategoryType[]> {
      const categoryTypes: CategoryType[] = [
         { Value: enCategoryType.Income, Text: await this.localization.GetTranslation(`categories.enCategoryType_Income`) },
         { Value: enCategoryType.Expense, Text: await this.localization.GetTranslation(`categories.enCategoryType_Expense`) }
      ];
      return categoryTypes;
   }

}
