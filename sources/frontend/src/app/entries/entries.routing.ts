import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';

const routes: Routes = [
   { path: 'edit/:entry', component: DetailsRouteViewComponent },
   { path: 'new/income', component: DetailsRouteViewComponent },
   { path: 'new/expense', component: DetailsRouteViewComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class EntriesRouting { }
