import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@elesse/identity';
import { HomeComponent } from './main/home/home.component';

const routes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', component: HomeComponent },
   { path: 'identity', loadChildren: () => import('./identity/identity.module').then(m => m.IdentityModule) },
   { path: 'accounts', canActivate: [AuthGuard], loadChildren: () => import('./accounts/accounts.module').then(m => m.AccountsModule) },
   { path: 'categories', canActivate: [AuthGuard], loadChildren: () => import('./categories/categories.module').then(m => m.CategoriesModule) },
   { path: 'entries', canActivate: [AuthGuard], loadChildren: () => import('./entries/entries.module').then(m => m.EntriesModule) },
   { path: '**', component: HomeComponent }
];

@NgModule({
   imports: [RouterModule.forRoot(routes)],
   exports: [RouterModule]
})
export class AppRouting { }
