import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

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

   constructor(
      private router: Router) { }

   // NAVIGATES
   public showList() { this.router.navigate(['/accounts']); }
   public showDetails(id: number) { this.router.navigate(['/account', id]); }
   public showNew() { this.router.navigate(['/account', 'new']); }

}
