import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { coreRoutes } from './core/core.routing';

@NgModule({
   imports: [RouterModule.forRoot(coreRoutes)],
   exports: [RouterModule]
})
export class AppRouting { }
