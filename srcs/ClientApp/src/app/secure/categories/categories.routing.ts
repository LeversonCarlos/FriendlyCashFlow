import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CategoriesComponent } from './categories.component';
import { CategoriesListComponent } from './categories-list/categories-list.component';
import { CategoryDetailsComponent } from './category-details/category-details.component';

const routes: Routes = [
   {
      path: '', component: CategoriesComponent,
      children: [
         { path: '', component: CategoriesListComponent },
         { path: ':id', component: CategoryDetailsComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class CategoriesRouting { }
