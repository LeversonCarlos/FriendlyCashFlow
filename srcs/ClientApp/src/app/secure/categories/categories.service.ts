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
   HierarchyText: string;
   SplitedText: string[];
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
   public showNew() { this.router.navigate(['/category', 'new']); }

   // CATEGORY TYPES
   public async getCategoryTypes(): Promise<EnumVM<enCategoryType>[]> {
      try {
         this.busy.show();
         const dataList = await this.http.get<EnumVM<enCategoryType>[]>("api/categories/types")
            .pipe(map(items => items.map(item => Object.assign(new EnumVM<enCategoryType>(), item))))
            .toPromise();
         return dataList;
      }
      catch (ex) { return null; }
      finally { this.busy.hide(); }
   }

}
