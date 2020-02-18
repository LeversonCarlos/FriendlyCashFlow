import { Component, OnInit } from '@angular/core';
import { CategoryType, CategoriesService, enCategoryType, Category } from '../categories.service';

@Component({
   selector: 'fs-categories-list',
   templateUrl: './categories-list.component.html',
   styleUrls: ['./categories-list.component.scss']
})
export class CategoriesListComponent implements OnInit {

   constructor(private service: CategoriesService) { }
   public Data: CategoryType[] = [];
   public SelectedTabIndex: number;

   public async ngOnInit() {
      this.Data = await this.service.getCategoryTypes();
      let tabIndex = 0;
      if (this.Data) {
         for (const item of this.Data) {
            if (item.Value == this.service.SelectedCategoryType) {
               this.SelectedTabIndex = tabIndex;
            }
            item.Categories = await this.service.getCategories(item.Value);
            tabIndex++;
         }
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
