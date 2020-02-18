import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

import { CategoriesRouting } from './categories.routing';
import { CategoriesService } from './categories.service';
import { CategoriesComponent } from './categories.component';
import { CategoriesListComponent } from './categories-list/categories-list.component';
import { CategoryDetailsComponent } from './category-details/category-details.component';

@NgModule({
   declarations: [CategoriesListComponent, CategoryDetailsComponent, CategoriesComponent],
   imports: [
      CommonModule, SharedModule,
      CategoriesRouting
   ],
   providers: [CategoriesService]
})
export class CategoriesModule { }
