import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryEntity } from '../../categories.data';
import { CategoriesService } from '../../categories.service';

@Component({
   selector: 'categories-list-body',
   templateUrl: './list-body.component.html',
   styleUrls: ['./list-body.component.scss']
})
export class ListBodyComponent implements OnInit {

   constructor(private service: CategoriesService) { }

   @Input()
   public Categories: Observable<CategoryEntity[]>

   ngOnInit(): void {
   }

   public OnRemoveCategory(category: CategoryEntity) {
      this.service.RemoveCategory(category);
   }

}
