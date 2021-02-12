import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEntity } from '../../model/categories.model';
import { CategoriesData } from '../../data/categories.data';

@Component({
   selector: 'categories-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor(private categoriesData: CategoriesData) { }

   @Input()
   public Categories: Observable<CategoryEntity[]>

   ngOnInit(): void {
   }

   public OnRemoveCategory(category: CategoryEntity) {
      this.categoriesData.RemoveCategory(category);
   }

}
