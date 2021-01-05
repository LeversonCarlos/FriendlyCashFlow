import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './main/home/home.component';

const routes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', component: HomeComponent },
   { path: 'identity', loadChildren: () => import('./identity/identity.module').then(m => m.IdentityModule) },
   { path: '**', component: HomeComponent }
];

@NgModule({
   imports: [RouterModule.forRoot(routes)],
   exports: [RouterModule]
})
export class AppRouting { }
