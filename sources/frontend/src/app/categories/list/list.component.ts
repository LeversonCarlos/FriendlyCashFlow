import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEntity, enCategoryType } from '../model/categories.model';
import { CategoriesData } from '../data/categories.data';

@Component({
   selector: 'categories-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private categoriesData: CategoriesData) { }

   public IncomeCategories: Observable<CategoryEntity[]>;
   public ExpenseCategories: Observable<CategoryEntity[]>;
   public get SelectedCategoryTab(): number { return this.categoriesData.SelectedType == enCategoryType.Expense ? 1 : 0; }

   public async ngOnInit() {
      this.IncomeCategories = this.categoriesData.ObserveCategories(enCategoryType.Income);
      this.ExpenseCategories = this.categoriesData.ObserveCategories(enCategoryType.Expense);
   }

   public OnTabChanged(tabIndex: number) {
      if (tabIndex == 0)
         this.categoriesData.SelectedType = enCategoryType.Income
      if (tabIndex == 1)
         this.categoriesData.SelectedType = enCategoryType.Expense
   }

}
