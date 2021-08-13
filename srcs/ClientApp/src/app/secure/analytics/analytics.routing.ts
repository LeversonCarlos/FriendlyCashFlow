import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AnalyticsComponent } from './analytics.component';

const routes: Routes = [
   { path: '', component: AnalyticsComponent },
   { path: ':year/:month', component: AnalyticsComponent },
   { path: ':year/:month/:printing', component: AnalyticsComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AnalyticsRouting { }
