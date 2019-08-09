import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { EnumVM } from 'src/app/shared/common/common.models';
import { BusyService } from 'src/app/shared/busy/busy.service';
import { map } from 'rxjs/operators';

export enum enCategoryType { None = 0, Expense = 1, Income = 2 };
export class Category {
   CategoryID: number;
   Text: string;
   Type: enCategoryType;
   ParentID: number;
   ParentRow?: Category;
   HierarchyText: string;
   SplitedText: string[];
}
export class CategoryType extends EnumVM<enCategoryType> {
   Categories: Category[] = []
}

@Injectable({
   providedIn: 'root'
})
export class CategoriesService {

   constructor(private busy: BusyService,
      private http: HttpClient, private router: Router) { }

   // NAVIGATES
   public showList() { this.router.navigate(['/categories']); }
   public showDetails(id: number) { this.router.navigate(['/category', id]); }
   public showNew(categoryType: enCategoryType) { this.router.navigate(['/category', `new-${categoryType}`]); }

   // CATEGORY TYPES
   public async getCategoryTypes(): Promise<CategoryType[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<CategoryType[]>("api/categories/types")
            .pipe(map(items => items.map(item => Object.assign(new CategoryType(), item))))
            .toPromise();
         return dataList;
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

}
