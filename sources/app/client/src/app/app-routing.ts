import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ElesseIdentityRoutes } from 'elesse-identity'

const routes: Routes = [];

@NgModule({
   imports: [RouterModule.forRoot(routes), RouterModule.forRoot(ElesseIdentityRoutes)],
   exports: [RouterModule]
})
export class AppRouting { }
