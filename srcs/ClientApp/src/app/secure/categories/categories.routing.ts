import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/core/auth/auth.guard';
import { CategoriesComponent } from './categories.component';
import { CategoriesListComponent } from './categories-list/categories-list.component';
import { CategoryDetailsComponent } from './category-details/category-details.component';

const routes: Routes = [
   {
      path: '', component: CategoriesComponent,
      children: [
         { path: '', redirectTo: 'list', pathMatch: 'full' },
         { path: 'list', canActivate: [AuthGuard], component: CategoriesListComponent },
         { path: ':id', canActivate: [AuthGuard], component: CategoryDetailsComponent },
      ]
   }
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class CategoriesRouting { }
