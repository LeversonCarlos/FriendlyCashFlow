import { Component, OnInit } from '@angular/core';
import { CategoriesService, CategoryType, Category, enCategoryType } from './categories.service';

@Component({
   selector: 'fs-categories',
   templateUrl: './categories.component.html',
   styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

   constructor(private service: CategoriesService) { }
   public Data: CategoryType[] = [];

   public async ngOnInit() {
      this.Data = await this.service.getCategoryTypes();
      for (const item of this.Data) {
         item.Categories = await this.service.getCategories(item.Value);
      }
   }

   private currentCategoryType: enCategoryType;
   public OnTypeSelected(tabIndex: number) {
      this.currentCategoryType = this.Data[tabIndex].Value;
   }

   public OnItemClick(item: Category) {
      this.service.showDetails(item.CategoryID);
   }

   public OnNewClick() {
      this.service.showNew(this.currentCategoryType);
   }

}
