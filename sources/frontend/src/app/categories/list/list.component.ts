import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEntity, enCategoryType } from '../categories.data';
import { CategoriesService } from '../categories.service';

@Component({
   selector: 'categories-list',
   templateUrl: './list.component.html',
   styleUrls: ['./list.component.scss']
})
export class ListComponent implements OnInit {

   constructor(private service: CategoriesService) { }

   public get IncomeCategories(): Observable<CategoryEntity[]> { return this.service.ObserveCategories(enCategoryType.Income); }
   public get ExpenseCategories(): Observable<CategoryEntity[]> { return this.service.ObserveCategories(enCategoryType.Expense); }
   public get SelectedCategoryTab(): number { return this.service.SelectedCategoryType == enCategoryType.Expense ? 1 : 0; }

   public async ngOnInit() {
      this.service.RefreshCache();
   }

   public OnTabChanged(tabIndex: number) {
      if (tabIndex == 0)
         this.service.SelectedCategoryType = enCategoryType.Income
      if (tabIndex == 1)
         this.service.SelectedCategoryType = enCategoryType.Expense
   }

}
