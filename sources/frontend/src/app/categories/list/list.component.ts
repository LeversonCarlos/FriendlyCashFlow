import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEntity, enCategoryType } from '../model/categories.model';
import { CategoriesService } from '../categories.service';

@Component({
   selector: 'categories-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: CategoriesService) { }

   public IncomeCategories: Observable<CategoryEntity[]>;
   public ExpenseCategories: Observable<CategoryEntity[]>;
   public get SelectedCategoryTab(): number { return this.service.SelectedCategoryType == enCategoryType.Expense ? 1 : 0; }

   public async ngOnInit() {
      this.IncomeCategories = this.service.ObserveCategories(enCategoryType.Income);
      this.ExpenseCategories = this.service.ObserveCategories(enCategoryType.Expense);
      this.service.RefreshCategories();
   }

   public OnTabChanged(tabIndex: number) {
      if (tabIndex == 0)
         this.service.SelectedCategoryType = enCategoryType.Income
      if (tabIndex == 1)
         this.service.SelectedCategoryType = enCategoryType.Expense
   }

}
