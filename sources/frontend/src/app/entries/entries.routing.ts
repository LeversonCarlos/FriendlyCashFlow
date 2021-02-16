import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';

const routes: Routes = [
   { path: 'details/:id', component: DetailsRouteViewComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class EntriesRouting { }
