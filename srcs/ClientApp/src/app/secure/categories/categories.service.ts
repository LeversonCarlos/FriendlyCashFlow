import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { map } from 'rxjs/operators';
import { enCategoryType, CategoryType, Category } from './categories.viewmodels';

@Injectable({
   providedIn: 'root'
})
export class CategoriesService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showList() { this.router.navigate(['/categories']); }
   public showDetails(id: number) { this.router.navigate(['/categories', id], { skipLocationChange: true }); }
   public showNew(categoryType: enCategoryType) { this.router.navigate(['/categories', `new-${categoryType}`], { skipLocationChange: true }); }

   // CATEGORY TYPES
   public SelectedCategoryType: enCategoryType = enCategoryType.None;
   public async getCategoryTypes(): Promise<CategoryType[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<CategoryType[]>("api/categories/types")
            .pipe(map(items => items.map(item => Object.assign(new CategoryType(), item))))
            .toPromise();
         return dataList;
         await this.LoadCategoryGoals(2021, 11);
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // CATEGORIES
   public async getCategories(categoryType: enCategoryType, searchText: string = ''): Promise<Category[]> {
      try {
         this.busy.show();
         let url = `api/categories/search/${categoryType}`;
         if (searchText) { url = `${url}/${encodeURIComponent(searchText)}`; }
         const dataList = await this.http.get<Category[]>(url)
            .pipe(map(items => items.map(item => Object.assign(new Category, item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // CATEGORY
   public async getCategory(categoryID: number): Promise<Category> {
      try {
         this.busy.show();
         const dataList = await this.http.get<Category>(`api/categories/${categoryID}`)
            .pipe(map(item => Object.assign(new Category, item)))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // SAVE
   public async saveCategory(value: Category): Promise<boolean> {
      try {
         this.busy.show();
         let result: Category = null;
         if (!value.CategoryID || value.CategoryID == 0) {
            result = await this.http.post<Category>(`api/categories`, value).toPromise();
         }
         else {
            result = await this.http.put<Category>(`api/categories/${value.CategoryID}`, value).toPromise();
         }
         return result != null;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   // REMOVE
   public async removeCategory(value: Category): Promise<boolean> {
      try {
         this.busy.show();
         const result = await this.http.delete<boolean>(`api/categories/${value.CategoryID}`).toPromise();
         return result;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

   public async LoadCategoryGoals(year: number, month: number): Promise<boolean> {
      const url = `api/analytics/categoryGoals/${year}/${month}`;
      const categoryGoals = await this.http.get<any[]>(url).toPromise();
      return null;
   }

}
