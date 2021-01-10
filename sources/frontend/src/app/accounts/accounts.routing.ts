import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';
import { DetailsRouteViewComponent } from './details/route-view/route-view.component';

const routes: Routes = [
   { path: '', redirectTo: 'list', pathMatch: 'full' },
   { path: 'list', component: ListComponent },
   { path: 'details/:id', component: DetailsRouteViewComponent },
   // { path: 'activate/:id/:code', component: ActivateComponent },
];

@NgModule({
   imports: [RouterModule.forChild(routes)],
   exports: [RouterModule]
})
export class AccountsRouting { }
