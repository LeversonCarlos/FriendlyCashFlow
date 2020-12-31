import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ElesseIdentityRoutes } from '@elesse/identity'
import { HomeComponent } from './home/home.component';

const routes: Routes = [
   { path: '', redirectTo: 'home', pathMatch: 'full' },
   { path: 'home', component: HomeComponent }
];

@NgModule({
   imports: [RouterModule.forRoot(routes), RouterModule.forRoot(ElesseIdentityRoutes)],
   exports: [RouterModule]
})
export class AppRouting { }
