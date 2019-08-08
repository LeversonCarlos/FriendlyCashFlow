import { Injectable } from '@angular/core';

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

   constructor() { }
}
