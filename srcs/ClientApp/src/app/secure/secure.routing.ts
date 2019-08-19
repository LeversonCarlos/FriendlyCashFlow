import { Routes } from '@angular/router';
import { AccountsComponent } from './accounts/accounts.component';
import { AccountDetailsComponent } from './accounts/account-details/account-details.component';
import { CategoriesComponent } from './categories/categories.component';
import { CategoryDetailsComponent } from './categories/category-details/category-details.component';
import { AuthGuard } from '../core/auth/auth.guard';

export const secureRoutes: Routes = [
   { path: 'accounts', canActivate: [AuthGuard], component: AccountsComponent },
   { path: 'account/:id', canActivate: [AuthGuard], component: AccountDetailsComponent },
   { path: 'categories', canActivate: [AuthGuard], component: CategoriesComponent },
   { path: 'category/:id', canActivate: [AuthGuard], component: CategoryDetailsComponent },
];
